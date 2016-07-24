using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace makemesmarter.Helpers
{
    public class ChitChat
    {
        public static string GetChitChatResponse(string query)
        {
            var finalQuery = string.Format(CultureInfo.InvariantCulture, Constants.ChitChatUrlFormat, query);
            HttpWebRequest request = WebRequest.Create(finalQuery) as HttpWebRequest;

            // Setting UA to simulate Cortana
            request.UserAgent = Constants.CortanaUserAgent;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader chitchatResponse = new StreamReader(response.GetResponseStream());

            XmlDocument document = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;

            string chitChatResultString = string.Empty;
            using (XmlReader reader = XmlReader.Create(chitchatResponse, settings))
            {
                document.Load(reader);
                XmlNodeList divs = document.GetElementsByTagName("h2");
                for (int i = 0; i < divs.Count; i++)
                {
                    var status = divs[i].Attributes["class"] != null ? divs[i].Attributes["class"].Value : null;
                    if (status != null && status.Contains("b_anno"))
                    {
                        chitChatResultString = divs[i].InnerText;
                    }
                }
            }

            return chitChatResultString;
        }
    }
}