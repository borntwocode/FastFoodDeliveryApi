using FastFoodDeliveryApi.Models.Entities.Abs;

namespace FastFoodDeliveryApi.Utils
{
    public class LocationIdentifier
    {
        private const string ApiKey = "f5a88abd8b514be6965d7161e0e30572";

        public static async Task<BaseAddress> GetLocation(double latitude, double longitude)
        {
            var geocoder = new OpenCage.Geocode.Geocoder(ApiKey);

            try
            {
                var result = await geocoder.ReverseGeocodeAsync(latitude, longitude);

                if (result == null || result.Results.Length <= 0)
                    return null;
                
                var location = result.Results[0];

                location.ComponentsDictionary.TryGetValue("city", out var city);
                location.ComponentsDictionary.TryGetValue("county", out var county);
                location.ComponentsDictionary.TryGetValue("residential", out var residential); 
                location.ComponentsDictionary.TryGetValue("road", out var road);

                var address = new BaseAddress()
                {
                    Id = Guid.NewGuid(),
                    City = city,
                    County = county,
                    Residential = residential,  
                    Road = road
                };
                return address;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
