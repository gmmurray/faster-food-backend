using faster_food_backend.Models;
using faster_food_backend.Models.Foursquare;
using faster_food_backend.Services.Abstractions;
using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace faster_food_backend.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // categories that are currently supported, could probably move to a json
        private readonly List<string> _availableCategoryNames = new List<string>()
        {
            "Chinese Restaurant",
            "Mexican Restaurant",
            "American Restaurant",
            "Asian Restaurant",
            "Japanese Restaurant",
            "Korean Restaurant",
            "Thai Restaurant",
            "Vietnamese Restaurant",
            "Bakery",
            "Bistro",
            "Breakfast Spot",
            "Bubble Tea Shop",
            "Coffee Shop",
            "Comfort Food Restaurant",
            "Dessert Shop",
            "Diner",
            "Fast Food Restaurant",
            "Food Truck",
            "Indian Restaurant",
            "Italian Restaurant",
            "Mediterranean Restaurant",
            "Middle Eastern Restaurant",
            "Salad Place",
            "Seafood Restaurant",
            "Southern / Soul Food Restaurant",
        };

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _versionDate;

        // DI
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public RestaurantService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            // Initialize client settings from secrets
            _clientId = _configuration["foursquare_client_id"];
            _clientSecret = _configuration["foursquare_client_secret"];
            _versionDate = _configuration["foursquare_version_date"];
        }

        public async Task<Result<IEnumerable<RestaurantResult>>> GetRestaurants(RestaurantRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Category))
                {
                    return Result.Fail("No category provided");
                }
                
                if (request.ZipCode.Length != 5 || !int.TryParse(request.ZipCode, out var zipCode))
                {
                    return Result.Fail("Invalid zip code");
                }

                var client = _httpClientFactory.CreateClient("foursquare");

                // Add client secrets to url
                var baseUrl = GetBaseUrlWithSecrets("explore");

                // Add request parameters to url
                var requestUrl = GetUrlWithExploreParameters(baseUrl, request);

                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUrl));

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Fail("Foursquare service error");
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FoursquareFullResponse>(stream, _jsonSerializerOptions);

                // Get venues from api response
                var venues = result.GetVenues();

                // Convert venues to result objects
                var restaurants = venues.Select(venue => new RestaurantResult(venue));

                return Result.Ok(restaurants);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<IEnumerable<FoursquareCategory>>> GetCategories()
        {
            try
            {
                var availableCategories = GetAvailableCategoryNames();

                var client = _httpClientFactory.CreateClient("foursquare");

                // Add client secrets to url
                var requestUrl = GetBaseUrlWithSecrets("categories");

                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUrl));

                // Get yo unreliability outta here
                if (!response.IsSuccessStatusCode)
                {
                    return Result.Fail("Foursquare service error");
                }

                // Stream response into json then deserialize
                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FoursquareCategoryResponse>(stream, _jsonSerializerOptions);

                // Filter categories result down to only food related categories
                var categoriesWithinFoodCategory = result.Response["categories"]
                    .FirstOrDefault(c => c.Name
                        .Equals("Food", StringComparison.CurrentCultureIgnoreCase))?
                        .Categories ?? new FoursquareCategory[0];

                // Filter those categories down further to only supported categories
                var matchingCategories = categoriesWithinFoodCategory
                    .Where(c => availableCategories.Contains(c.Name.ToLower()));

                // Removes any child categories within each category because they won't be used
                foreach (var category in matchingCategories)
                {
                    category.PurgeChildCategories();
                }

                return Result.Ok(matchingCategories);
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        private string GetBaseUrlWithSecrets(string baseUrl)
        {
            return baseUrl
                + $"?client_id={_clientId}"
                + $"&client_secret={_clientSecret}"
                + $"&v={_versionDate}";
        }

        private string GetUrlWithExploreParameters(string baseUrl, RestaurantRequest request)
        {
            return baseUrl
                + $"&near={request.ZipCode}"
                + $"&categoryId={request.Category}"
                + $"&limit={request.ResultCount}";
        }

        private IEnumerable<string> GetAvailableCategoryNames()
        {
            return _availableCategoryNames.Select(name => name.ToLower());
        }
    }
}
