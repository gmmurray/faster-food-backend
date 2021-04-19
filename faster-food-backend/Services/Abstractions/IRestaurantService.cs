using faster_food_backend.Models;
using faster_food_backend.Models.Foursquare;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace faster_food_backend.Services.Abstractions
{
    public interface IRestaurantService
    {
        Task<Result<IEnumerable<RestaurantResult>>> GetRestaurants(RestaurantRequest request);
        Task<Result<IEnumerable<FoursquareCategory>>> GetCategories();
    }
}
