using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QueryIntent = makemesmarter.Helpers.Constants.Intents;

namespace makemesmarter.Helpers
{
    public class BingDataApis
    {
        public static string GetData(QueryIntent intent, string query)
        {
            switch(intent)
            {
                case QueryIntent.CHITCHAT:
                    return ChitChat.GetChitChatResponse(query);
            }

            return string.Empty;
        }
    }
}