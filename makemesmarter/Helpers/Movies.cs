using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    public class Movies
    {
        public static async Task<string> GetMoviesResult(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BingDataApiBaseUrl);

                var moviesUrl = string.Format(Constants.BingDataApiUrl, query, Constants.BingDataApiAppID);
                var response = await CallEndpoint(client, moviesUrl);

                var movieJsonResponse = JsonConvert.DeserializeObject<CommonSchemas.EntitiesApiResult>(response);
                if (movieJsonResponse != null && movieJsonResponse.entities != null && movieJsonResponse.entities.value != null && movieJsonResponse.entities.value.Count > 0)
                {
                    var movieRating = GetRating(movieJsonResponse.entities.value[0]);

                    return !string.IsNullOrWhiteSpace(movieRating) ? movieRating : movieJsonResponse.entities.value[0].description;
                }

                return null;
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