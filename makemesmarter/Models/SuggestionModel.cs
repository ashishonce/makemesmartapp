using System;
using System.Collections.Generic;
using System.Net;
using makemesmarter.Helpers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace makemesmarter.Models
{
    public class SuggestionData
    {
        public bool resultValid;
        public string userMessage;
        public string replyMessage;
        public int userMood;
        public string nextSuggestedReply;
    }

    public static class SuggestionModel
    {
        public static async Task<SuggestionData>  GetSuggestions(string queryString)
        {
            var suggestion = new SuggestionData();

            if (string.IsNullOrWhiteSpace(queryString))
            {
                // add the business logic here 
                suggestion.replyMessage = "EMPTY";
                suggestion.resultValid = false;
            }
            else
            {
                suggestion.userMessage = queryString;
            }

            // here i call LUIS/ANYTHING to populate the intent so that we decide how to proceed
            var url = "https://api.projectoxford.ai/luis/v1/application?id=30aa83c1-7760-4a7d-84db-fd9a31a451ed&subscription-key=af79ebce73804e53b12f797a6cfc3909&q={0}";
            url = string.Format(url, queryString);
            string intent = string.Empty;
            IList<string> Entities = new List<string>();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                JObject jsonObject = JObject.Parse(json);
                foreach (var entity in jsonObject["entities"])
                {
                    Entities.Add(entity["entity"].ToString());
                }

                intent = (jsonObject["intents"][0])["intent"].ToString();
            }

            var extractedIntent = GetIntent(intent);
            IList<string> keyPhrases = null;
            string bingData = string.Empty;

            if (Entities.Count > 0)
            {
                foreach (var entity in Entities)
                {
                    keyPhrases.Add(entity);
                }
            }

            // Call keyphrases if the intent is OTHERS
            if (Entities.Count == 0)
            {
                keyPhrases = KeyPhrases.GetKeyPhrases(queryString).Result;
            }
            else
            {
                bingData = BingDataApis.GetData(extractedIntent, queryString);
            }

            suggestion.replyMessage = bingData;
            // Call sentiment detector to get the score
            var sentimentLevel = await SentimentDetector.GetSentiment(queryString);

            // Call Mood calculator to get the appropriate strings
            suggestion.nextSuggestedReply = MoodCalculator.getMoodString(sentimentLevel);

            return suggestion;
        }

        private static Constants.Intents GetIntent(string intent)
        {
            switch(intent)
            {
                case "Sports":
                    return Constants.Intents.Sports;
                    
                case "Find information":
                    return Constants.Intents.FindInfo;
                    
                case "Movies":
                    return Constants.Intents.MOVIES;
                    
                case "PersonalChat":
                    return Constants.Intents.CHITCHAT;
                    
                case "News":
                    return Constants.Intents.NEWS;
                    
                case "Translation":
                    return Constants.Intents.Translation;
    
                case "Weather":
                    return Constants.Intents.Weather;
                    
                case "None":
                    return Constants.Intents.OTHERS;
                    
            }

            return Constants.Intents.OTHERS;
        }
    }
}