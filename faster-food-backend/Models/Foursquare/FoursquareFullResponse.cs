using System.Collections.Generic;
using System.Linq;

namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareFullResponse
    {
        public object Meta { get; set; }
        public FoursquareResponse Response { get; set; }

        public IEnumerable<FoursquareVenue> GetVenues()
        {
            return Response?.Groups?.FirstOrDefault()?.Items.Select(item => item?.Venue) ?? new FoursquareVenue[0];
        }
    }
}
