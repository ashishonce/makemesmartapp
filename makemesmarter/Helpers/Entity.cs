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
                var entityDescription = entityJsonResponse.entities.value[0].description;

                return entityDescription;
            }
        }

        static async Task<String> CallEndpoint(HttpClient client, string uri)
        {
            var response = await client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }

        private static string GetRating(CommonSchemas.EntityResultValue movieEntity)
        {
            string movieFacts = string.Empty;

            if (movieEntity.aggregateRating != null)
            {
                movieFacts = "AggregateRating: " + movieEntity.aggregateRating.ratingValue + "/" + movieEntity.aggregateRating.bestRating;
                return movieFacts;
            }

            if (movieEntity.entityPresentationInfo.formattedFacts == null || movieEntity.entityPresentationInfo.formattedFacts.Count == 0)
            {
                return null;
            }

            foreach (var formattedFact in movieEntity.entityPresentationInfo.formattedFacts)
            {
                if (string.IsNullOrWhiteSpace(formattedFact.label) || formattedFact.label == "Summary")
                {
                    foreach (var item in formattedFact.items)
                    {
                        if (!string.IsNullOrWhiteSpace(movieFacts))
                        {
                            movieFacts += " ";
                        }

                        movieFacts += item.text;
                    }
                }
            }

            return movieFacts;
        }
    }
}