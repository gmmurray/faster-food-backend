using System.Collections.Generic;

namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareCategoryResponse
    {
        public object Meta { get; set; }
        public Dictionary<string, FoursquareCategory[]> Response { get; set; }
    }
}
