namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareVenueLocation
    {
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string[] FormattedAddress { get; set; } = new string[0];
    }
}
