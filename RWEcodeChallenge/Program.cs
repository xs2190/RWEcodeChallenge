using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HttpClientSample
{
    //Would like to separate this into different files
    public class WindPark
    {//nullable required for compilation, re-define properties once the API GET is figured out
        public string? name { get; set; }
        public string? description { get; set; }
        public int ID { get; set; }
        public string? region { get; set; }
        public string? country { get; set; }
        public List<Turbine>? turbines;//might change to dictionary if possible

        public class Turbine
        { //nullable required for compilation, re-define properties once the API GET is figured out
            public int? id;
            public string? name;
            public string? manufacturer;
            public string? version;
            public float? maxProduction;
            public float? currentProduction;
            public float? windspeed;
            public string? windDirection;
        }
    }


    class Program //Would like to separate this into different files
    {
        static HttpClient client = new HttpClient();

        static async Task<WindPark> GetWPAsync(string path)
        {
            /*
             * 0. Figure out how to call the API
             * 1. get the content from the API
             * 2. determine how to parse the API response
             * 3. put the API information into a WindPark object
             * 4. fill in the turbine information using a turbine object
            */

            //0
            HttpResponseMessage response = await client.GetAsync("/api/Site");

            response.EnsureSuccessStatusCode();
            //var test = response.Content.Headers.ContentType;
            //var test2 = response.Content.ToString();
            //var test4 = response.Content.ReadAsStream();

            //1a ==> works for plain/text but is messy and innefficient
            var test3 = response.Content.ReadAsStringAsync();

            //1b ==> try reading & deserializing as JSON
            //var test5 = response.Content.ReadFromJsonAsync(); ==> no
            //https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
            


            response.Content.ReadAsStringAsync().Wait();

            return new WindPark(); //needed to compile, not finished product
        }


        static void Main()
        {
            /*
             * 1. Get all WindPark site IDs into WindPark objects
             * 2. Get turbine information for each site
             * 3. Resync every 5 minutes to pull current turbine information
             * 4. Send data to RabbitMQ Exchange with a que
             */
            RunAsync().GetAwaiter().GetResult();

        }

        static async Task RunAsync()
        {
            try
            {
                client.BaseAddress = new Uri("http://renewables-codechallenge.azurewebsites.net/swagger/index.html");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //changed to plain/text to see if it is easier to parse --> no changing back to application/json

                WindPark wp = await GetWPAsync(client.BaseAddress.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }
}