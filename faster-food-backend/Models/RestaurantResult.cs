using faster_food_backend.Models.Foursquare;

namespace faster_food_backend.Models
{
    public class RestaurantResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string DeliveryUrl { get; set; }

        public RestaurantResult() { }

        public RestaurantResult(FoursquareVenue venue)
        {
            Id = venue.Id;
            Name = venue.Name;
            Address = venue.Location.Address;
            PostalCode = venue.Location.PostalCode;
            City = venue.Location.City;
            State = venue.Location.State;
            DeliveryUrl = venue.Delivery?.Url;
        }
    }
}
