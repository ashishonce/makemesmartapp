using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    public class NewsApiResult
    {
        public string _type { get; set; }

        public NewsResult news { get; set; }
    }

    public class NewsResult
    {
        public string id { get; set; }

        public string readLink { get; set; }

        public IList<NewsResultValue> value { get; set; }
    }

    public class NewsResultValue
    {
        public string name { get; set; }

        public string url { get; set; }

        public CommonSchemas.Image image { get; set; }

        public string description { get; set; }

        public IList<About> about { get; set; }

        public IList<Provider> provider { get; set; }

        public string datePublished { get; set; }

        public string category { get; set; }

        public IList<NewsResultValue> clusteredArticles { get; set; }
    }

    public class About
    {
        public string readLink { get; set; }

        public string name { get; set; }
    }

    public class Provider
    {
        public string _type { get; set; }

        public string name { get; set; }
    }

    public class News
    {
        public static async Task<string> GetNewsResult(string query)
        {
            using (var client = new HttpClient())
            {                
                client.BaseAddress = new Uri(Constants.BingDataApiBaseUrl);

                var newsUrl = string.Format(Constants.BingDataApiUrl, query, Constants.BingDataApiAppID);
                var response = await CallEndpoint(client, newsUrl);

                var newsJsonResponse = JsonConvert.DeserializeObject<NewsApiResult>(response);

                if (newsJsonResponse != null
                    && newsJsonResponse.news != null
                    && newsJsonResponse.news.value != null
                    && newsJsonResponse.news.value.Count > 0
                    && !string.IsNullOrWhiteSpace(newsJsonResponse.news.value[0].name))
                {
                    return newsJsonResponse.news.value[0].name;
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