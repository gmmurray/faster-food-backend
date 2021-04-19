namespace faster_food_backend.Models.Foursquare
{
    public class FoursquareCategory
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public FoursquareCategoryIcon Icon { get; set; }
        public FoursquareCategory[] Categories { get; set; }
        public string IconUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Icon.Prefix) || string.IsNullOrEmpty(Icon.Suffix))
                {
                    return string.Empty;
                }

                return GetUrl();
            }
        }

        private string GetUrl()
        {
            var prefix = Icon.Prefix.Contains("ss3.4sqi.net")
                ? Icon.Prefix.Replace("ss3.4sqi.net", "foursquare.com")
                : Icon.Prefix;
            return prefix + "bg_88" + Icon.Suffix;
        }

        public void PurgeChildCategories()
        {
            Categories = null;
        }
    }
}
