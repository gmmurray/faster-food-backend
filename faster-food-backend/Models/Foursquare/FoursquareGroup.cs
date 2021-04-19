namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareGroup
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public FoursquareGroupItem[] Items { get; set; } = new FoursquareGroupItem[0];
    }
}
