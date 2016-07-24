using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    // Classes to store the key phrases through text analysis
    internal class KeyPhrasesResult
    {
        internal List<KeyPhrasesResultDocument> documents { get; set; }
    }

    internal class KeyPhrasesResultDocument
    {
        internal List<string> keyPhrases { get; set; }

        internal string id { get; set; }
    }

    public class KeyPhrases
    {
        public static async Task<IList<string>> GetKeyPhrases(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.AzureApiBaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add(Constants.AzureApiAccountKeyHeader, Constants.AzureApiAccountKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" +
                    "{\"id\":\"1\",\"text\":\"" + query + "\"},]}");

                // Detect sentiment:
                var uri = "text/analytics/v2.0/keyPhrases";
                var response = await CallEndpoint(client, uri, byteData);

                var sentimentJsonResponse = JsonConvert.DeserializeObject<KeyPhrasesResult>(response);
                var sentimentScore = sentimentJsonResponse.documents[0].keyPhrases;

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