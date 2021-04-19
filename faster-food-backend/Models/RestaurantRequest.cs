namespace faster_food_backend.Models
{
    public class RestaurantRequest
    {
        public string ZipCode { get; set; }
        public string Category { get; set; }
        public int ResultCount { get; set; }
    }
}
