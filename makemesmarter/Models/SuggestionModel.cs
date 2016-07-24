using System.Collections.Generic;
using makemesmarter.Helpers;
using System.Threading.Tasks;

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
            var extractedIntent = Constants.Intents.CHITCHAT;
            IList<string> keyPhrases = null;
            string bingData = string.Empty;

            // Call keyphrases if the intent is OTHERS
            if (extractedIntent == Constants.Intents.OTHERS)
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
    }
}