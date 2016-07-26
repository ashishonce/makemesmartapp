using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    public class Entity
    {
        public static async Task<string> GetEntityResult(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BingDataApiBaseUrl);

                var entityUrl = string.Format(Constants.BingDataApiUrl, query, Constants.BingDataApiAppID);
                var response = await CallEndpoint(client, entityUrl);

                var entityJsonResponse = JsonConvert.DeserializeObject<CommonSchemas.EntitiesApiResult>(response);

                if (entityJsonResponse != null 
                    && entityJsonResponse.entities != null 
                    && entityJsonResponse.entities.value != null 
                    && entityJsonResponse.entities.value.Count > 0 
                    && !string.IsNullOrWhiteSpace(entityJsonResponse.entities.value[0].description))
                {
                    return entityJsonResponse.entities.value[0].description;
                }

                return null;
            }
        }

        static async Task<String> CallEndpoint(HttpClient client, string uri)
        {
            var response = await client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }
    }
}