using Cowboytask.Database.Models;
using Cowboytask.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Cowboytask.Helper;

public class ExternalServiceHelper
{
    public static List<Firearm> FirearmsCollection = new List<Firearm>
    {
        new Firearm{GunName = "Pistol", MaxNumOfBullets = 6, RemainedNumOfBullets = 6},
        new Firearm{GunName = "Machine Gun", MaxNumOfBullets = 150, RemainedNumOfBullets = 150},
        new Firearm{GunName = "Revolvers", MaxNumOfBullets = 30, RemainedNumOfBullets = 30},
        new Firearm{GunName = "Launchers", MaxNumOfBullets = 200, RemainedNumOfBullets = 110},
        new Firearm{GunName = "Rifles", MaxNumOfBullets = 30, RemainedNumOfBullets = 25},
        new Firearm{GunName = "Shotguns", MaxNumOfBullets = 20, RemainedNumOfBullets = 20},
        new Firearm{GunName = "Assault Rifles", MaxNumOfBullets = 50, RemainedNumOfBullets = 50}
    };

    public static GetCowboyResponseModel GetFakeCowboy()
    {
        GetCowboyResponseModel dummyCowboy = null;
        try
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("https://api.namefake.com/").Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsStringAsync().Result;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                dummyCowboy = JsonSerializer.Deserialize<GetCowboyResponseModel>(res, options);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while fetching cowboy from external service. Exception message is " + ex.Message);
        }
        return dummyCowboy;
    }

    public static List<Firearm> GetFirearms()
    {
        List<Firearm> firearms = new List<Firearm>();
        var random = new Random();
        var numOfFirearms = random.Next(1, 4);
        for (int i = 1; i <= numOfFirearms; i++)
        {
            var itemIndex = random.Next(0, FirearmsCollection.Count);
            firearms.Add(new Firearm
            {
                 GunName = FirearmsCollection[itemIndex].GunName,
                 MaxNumOfBullets = FirearmsCollection[itemIndex].MaxNumOfBullets,
                 RemainedNumOfBullets = FirearmsCollection[itemIndex].RemainedNumOfBullets
            });
        }
        return firearms;
    }
}
