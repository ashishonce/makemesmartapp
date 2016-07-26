using System.Threading.Tasks;
using QueryIntent = makemesmarter.Helpers.Constants.Intents;

namespace makemesmarter.Helpers
{
    public class BingDataApis
    {
        public static async Task<string> GetData(QueryIntent intent, string query)
        {
            switch(intent)
            {
                case QueryIntent.CHITCHAT:
                    return ChitChat.GetChitChatResponse(query);
                case QueryIntent.MOVIES:
                    return await Movies.GetMoviesResult(query);
                case QueryIntent.NEWS:
                    return await News.GetNewsResult(query);
                case QueryIntent.SPORTS:
                case QueryIntent.FINDINFO:
                    return await Entity.GetEntityResult(query);
                case QueryIntent.CONTACT:
                    return await Command.GetContactEntity(query);
                case QueryIntent.CALENDAR:
                    return await Command.GetCalendarEntity(query);
                default:
                    return await Task.FromResult<string>(query);
            }
        }
    }
}