namespace makemesmarter.Helpers
{
    public class Constants
    {
        public const string AzureApiBaseUrl = "https://westus.api.cognitive.microsoft.com/";

        public const string AzureApiAccountKey = "6fce4869ab614b98997e2ed8cba663a3";

        public const string AzureApiAccountKeyHeader = "Ocp-Apim-Subscription-Key";

        public const string ChitChatUrlFormat = "https://www.bing.com/search?q={0}&p1=%5BChitChat%20UserId=%22310%22%5D";

        public const string CortanaUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Trident/7.0; Touch; rv:11.0; Cortana 1.6.1.52; 10.0.0.0.10586.21) like Gecko";

        public const string BingDataApiBaseUrl = "https://www.bingapis.com/";

        public const string BingDataApiUrl = "api/v5/search?q={0}&appid={1}";

        public const string BingDataApiAppID = "FA15B041D8404F934D4866D461005AE5159F0A90";

        public const string LUISUrlFormat = "https://api.projectoxford.ai/luis/v1/application?id=30aa83c1-7760-4a7d-84db-fd9a31a451ed&subscription-key=af79ebce73804e53b12f797a6cfc3909&q={0}";

        public enum Intents
        {
            CHITCHAT = 0,
            ENTITY,
            PLACE,
            NEWS,
            MOVIES,
            SPORTS,
            FINDINFO,
            TRANSLATION,
            WEATHER,
            CONTACT,
            CALENDAR,
            OTHERS
        }

        public enum PossibleMoods
        {
            GRIEF = 0,
            SAD,
            AWFULL,
            NONE,
            NEUTRAL,
            HAPPY,
            VERYHAPPY,
            JUBLIENT,
        }
    }
}