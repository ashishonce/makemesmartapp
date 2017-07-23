using System.Collections.Generic;
using System.Linq;
using System.Net;
using makemesmarter.Helpers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using makemesmarter.Models;
using Newtonsoft.Json;


namespace makemesmarter.Models
{
    using System;
    using System.Net;

    public class WebDownload : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebDownload() : this(10000) { }

        public WebDownload(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }

    public class SuggestionData
    {        
        public string userMessage { get; set; }

        public double userMoodPercentage { get; set; }

        public bool isValid { get; set; }

        public IList<QueryEntity> queryEntities { get; set; }

        public Constants.Intents Intent { get; set; }
    }

    public class QueryEntity

    {
        public string entityName { get; set; }

        public string fullBingData { get; set; }

        public string suggestedReply { get; set; }
    }

    public static class SuggestionModel
    {
        public static async Task<SuggestionData>  GetSuggestions(string queryString)
        {
            var suggestion = new SuggestionData();
            
            // No op when there is no query string
            if (string.IsNullOrWhiteSpace(queryString))
            {
                suggestion.isValid = false;
                return suggestion;
            }

            suggestion.userMessage = queryString;
            suggestion.userMoodPercentage = await SentimentDetector.GetSentiment(queryString);

            IList<string> entities = new List<string>();
            var extractedIntent = GetLUISIntent(queryString, entities);
            suggestion.Intent = extractedIntent;
            if (extractedIntent == Constants.Intents.CHITCHAT)
            {
                entities = new List<string>() { queryString };
            }

            // Even after calling Luis if the entities are empty, call keyphrases api to get key phrases
           
            if (extractedIntent == Constants.Intents.OTHERS)
            {
                entities = new List<string>() { };
            }
            else if (entities.Count == 0)
            {
                entities = await KeyPhrases.GetKeyPhrases(queryString);
            }

            // Extract distinct entities
            entities = entities.Distinct().ToList();
            suggestion.queryEntities = new List<QueryEntity>();
            foreach (var entity in entities)
            {
                var queryEntity = new QueryEntity();
                queryEntity.entityName = entity;
                queryEntity.fullBingData = await BingDataApis.GetData(extractedIntent, entity);
                queryEntity.suggestedReply = MoodCalculator.getMoodString(suggestion.userMoodPercentage);

                suggestion.queryEntities.Add(queryEntity);
            }

            return suggestion; 
        }

        private static Constants.Intents GetLUISIntent(string queryString, IList<string> entities)
        {
            try {
                // Call Luis to get the intent and entities for a given query
                var luisUrl = string.Format(Constants.LUISUrlFormat, queryString);
                string intent = string.Empty;
                using (WebClient wc = new WebDownload())
                {
                    var json = wc.DownloadString(luisUrl);
                    JObject jsonObject = JObject.Parse(json);
                    foreach (var entity in jsonObject["entities"])
                    {
                        entities.Add(entity["entity"].ToString());
                    }

                    intent = (jsonObject["intents"][0])["intent"].ToString();
                }

                switch (intent)
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

                    case "Command":
                        return Constants.Intents.CONTACT;

                    case "Calender":
                        return Constants.Intents.CALENDAR;

                    default:
                        return Constants.Intents.OTHERS;
                }
            }
            catch (Exception)
            {
                return Constants.Intents.OTHERS;
            }
            }
    }
}