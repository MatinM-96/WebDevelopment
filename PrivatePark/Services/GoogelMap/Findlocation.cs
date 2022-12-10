using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mnacr22.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Results = mnacr22.Models.Results;

namespace mnacr22.Services;

public class FindLocation
{
    /*
    private static string? _googleKey;

    static FindLocation()
    {
        var options = new AuthMessageSenderOptions().GoogleKey;

        _googleKey = options;
    }
    */
    public static async Task<string> GetTheLatitudeAndLongitude(string city, string street, string zipcode)

    {
        
        
        string URL = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}+{1},+{2}&key={3}", street, city, zipcode, ApiKey.apikey());
        using (HttpClient client = new HttpClient())
        {

            var response = await client.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return json; 
                
            }

            return null;
        }
    }
    
    
    
    
    
}

