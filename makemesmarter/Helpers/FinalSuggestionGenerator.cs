﻿using System;
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
                    var sugg = FormSuggestion(entity, suggestionData.Intent, suggestionData.userMessage);
                    var separator = (suggestionData.Intent == Constants.Intents.CONTACT) || (suggestionData.Intent == Constants.Intents.CALENDAR) ? "~" : "$";
                    if (sugg != null)
                    {
                        finalString += separator + sugg ;
                    }
                }
            }

            return finalString;
        }

        public static string FormSuggestion(QueryEntity entity, Constants.Intents intent, string message)
        {
            if (entity != null && entity.fullBingData != null)
            {
                int index = -1;
                if (intent == Constants.Intents.NEWS || intent == Constants.Intents.SPORTS || intent == Constants.Intents.FINDINFO)
                {
                    index = entity.fullBingData.IndexOf(".") > 0 ? entity.fullBingData.IndexOf(".") : entity.fullBingData.Length;
                }
                else
                {
                    index = entity.fullBingData.Length;
                }
                var substring = entity.fullBingData.Substring(0, index);
                if (intent != Constants.Intents.CONTACT && intent != Constants.Intents.CALENDAR)
                {
                    var emoji = "";
                    if (intent == Constants.Intents.CHITCHAT && message.ToLowerInvariant().Contains("like") || message.ToLowerInvariant().Contains("love") || message.ToLowerInvariant().Contains("thanks"))
                    {
                        emoji = " 😍";
                    }
                    if (intent == Constants.Intents.CHITCHAT && message.ToLowerInvariant().Contains("hi") || message.ToLowerInvariant().Contains("hello"))
                    {
                        emoji = " 😀";
                    }
                    if (intent == Constants.Intents.NEWS && entity.suggestedReply.ToLowerInvariant().Contains("terrible"))
                    {
                        emoji = " 😢";
                    }

                    substring += emoji;
                }

                return substring;
            }

            return null;
        }
    }
}