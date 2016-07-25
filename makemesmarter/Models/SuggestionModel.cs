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
        public double userMood;
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

            // Call Luis to get the intent and entities for a given query
            var luisUrl = string.Format(Constants.LUISUrlFormat, queryString);

            string intent = string.Empty;
            IList<string> entities = new List<string>();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(luisUrl);
                JObject jsonObject = JObject.Parse(json);
                foreach (var entity in jsonObject["entities"])
                {
                    entities.Add(entity["entity"].ToString());
                }

                intent = (jsonObject["intents"][0])["intent"].ToString();
            }

            var extractedIntent = GetIntent(intent);            
            string bingData = string.Empty;

            // Even after calling Luis if the entities are empty, call keyphrases api to get key phrases
            if (entities.Count == 0)
            {
                entities = await KeyPhrases.GetKeyPhrases(queryString);
            }

            var modifiedQuery = entities != null && entities.Count > 0 && !string.IsNullOrWhiteSpace(entities[0]) ? entities[0] : queryString;
            bingData = await BingDataApis.GetData(extractedIntent, queryString);
            suggestion.replyMessage = bingData;            

            // Call sentiment detector to get the score
            var sentimentLevel = await SentimentDetector.GetSentiment(queryString);
            suggestion.userMood = sentimentLevel;

            // Call Mood calculator to get the appropriate strings
            suggestion.nextSuggestedReply = MoodCalculator.getMoodString(sentimentLevel);

            return suggestion;
        }

        private static Constants.Intents GetIntent(string intent)
        {
            switch(intent)
            {
                case "Sports":
                    return Constants.Intents.SPORTS;
                    
                case "Find information":
                    return Constants.Intents.FINDINFO;
                    
                case "Movies":
                    return Constants.Intents.MOVIES;
                    
                case "PersonalChat":
                    return Constants.Intents.CHITCHAT;
                    
                case "News":
                    return Constants.Intents.NEWS;
                    
                case "Translation":
                    return Constants.Intents.TRANSLATION;
    
                case "Weather":
                    return Constants.Intents.WEATHER;
                    
                case "None":
                    return Constants.Intents.OTHERS;

                default:
                    return Constants.Intents.OTHERS;
            }            
        }
    }
}