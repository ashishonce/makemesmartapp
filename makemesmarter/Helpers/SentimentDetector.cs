using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    // Classes to store the result from the sentiment analysis
    public class SentimentResult
    {
        public List<SentimentResultDocument> documents { get; set; }
    }

    public class SentimentResultDocument
    {
        public double score { get; set; }

        public string id { get; set; }
    }

    public class SentimentDetector
    {
        public static async Task<double> GetSentiment(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.AzureApiBaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add(Constants.AzureApiAccountKeyHeader, Constants.AzureApiAccountKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" + "{\"id\":\"1\",\"text\":\"" + query + "\"},]}");

                // Detect sentiment:
                var uri = "text/analytics/v2.0/sentiment";
                var response = await CallEndpoint(client, uri, byteData);

                var sentimentJsonResponse = JsonConvert.DeserializeObject<SentimentResult>(response);
                var sentimentScore = sentimentJsonResponse.documents[0].score * 100;

                return sentimentScore;
            }
        }

        static async Task<String> CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}