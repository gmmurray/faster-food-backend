namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareVenue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public FoursquareVenueLocation Location { get; set; }
        public FoursquareVenueDelivery Delivery { get; set; }
    }
}
