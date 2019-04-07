using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Places
{
    public static class Api
    {
        private static HttpClient Client { get; set; }
        public static string AppSecret = "YOUR-API-KEY-HERE"; // App Key

        static Api()
        {
            Client = new HttpClient {BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/")};
        }


        /// <summary>
        /// Gets stores of a certain category near the specified co-ordinates. Can be show only
        /// open stores or all stores.
        /// </summary>
        /// <param name="latitude">Latitude of user.</param>
        /// <param name="longitude">Longitude of user.</param>
        /// <param name="category">Category of stores to find.</param>
        /// <param name="open">Should find only open stores.</param>
        public static async Task<Response> GetPlaces(double latitude, double longitude, Category category, bool onlyOpen)
        {
            string url = null;
            if (onlyOpen)
            {
                url =
                    $"nearbysearch/json?key={AppSecret}&location={latitude},{longitude}&sensor=true&rankby=distance&types={category.ToString()}&opennow";
            }
            else
            {
                url =
                    $"nearbysearch/json?key={AppSecret}&location={latitude},{longitude}&sensor=true&rankby=distance&types={category.ToString()}";
            }
            try
            {
                var resp = await Client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), typeof(Response)) as Response;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Gets another set of 20 places that match a previous query.
        /// </summary>
        /// <param name="token">Next Token to fetch.</param>
        public static async Task<Response> GetNext(string token)
        {
            try
            {
                var resp = await Client.GetAsync($"nearbysearch/json?key={AppSecret}&sensor=true&pagetoken={token}");
                if (resp.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), typeof(Response)) as Response;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Locale search near specified co-ordinates.
        /// </summary>
        /// <param name="latitude">Latitude of user.</param>
        /// <param name="longitude">Longitude of user.</param>
        /// <param name="query">Search query</param>
        public static async Task<Response> SearchPlaces(double latitude, double longitude, string query)
        {
            try
            {
                var resp = await Client.GetAsync(
                    $"nearbysearch/json?key={AppSecret}&location={latitude},{longitude}&sensor=true&rankby=distance&keyword={query}");
                if (resp.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), typeof(Response)) as Response;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get full details of specified place ID.
        /// </summary>
        /// <param name="placeId">ID of place.</param>
        public static async Task<Detail> GetDetails(string placeId)
        {
            try
            {
                var resp = await Client.GetAsync($"details/json?key={AppSecret}&placeid={placeId}&sensor=true");
                return resp.IsSuccessStatusCode ? (JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), typeof(Response)) as Response)?.Detail : null;
            }
            catch
            {
                return null;
            }
        }
    }
}