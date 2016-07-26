using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using makemesmarter.Models;

namespace makemesmarter.Helpers
{
    // Classes to store the key phrases through text analysis
    public class FinalSuggestionGenerator
    {
        public static string Generate(SuggestionData suggestionData)
        {
            var finalString = suggestionData.userMessage;
            if (suggestionData != null && suggestionData.queryEntities != null)
            {
                foreach (var entity in suggestionData.queryEntities)
                {
                    var sugg = FormSuggestion(entity);
                    if (sugg != null)
                    {
                        finalString += "$" + sugg;
                    }
                }
            }

            return finalString;
        }

        public static string FormSuggestion(QueryEntity entity)
        {
            if (entity != null && entity.fullBingData != null)
            {
                var index = entity.fullBingData.IndexOf(".") > 0 ? entity.fullBingData.IndexOf(".") : entity.fullBingData.Length;
                var substring = entity.fullBingData.Substring(0, index);
                if (!entity.suggestedReply.Equals("NONE"))
                {
                    substring += ", " + entity.suggestedReply;
                }

                return substring;
            }

            return null;
        }
    }
}