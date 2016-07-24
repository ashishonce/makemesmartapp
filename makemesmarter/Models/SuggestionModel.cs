using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using makemesmarter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public static SuggestionData GetSuggestions(int? moods, string queryString)
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

            // here i call LUIS  / ANYTHING

            suggestion.nextSuggestedReply = MoodCalculator.getMoodString((float)moods);
            // result 

            return suggestion;
        }


    }
}