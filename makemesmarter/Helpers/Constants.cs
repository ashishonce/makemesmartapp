namespace makemesmarter.Helpers
{
    public class Constants
    {
        public const string AzureApiBaseUrl = "https://westus.api.cognitive.microsoft.com/";

        // another key: e3ee1ecbc1ea400ea5f77653a2844744
        public const string AzureApiAccountKey = "3f0456f3b5c843129a861fde8a68080e";

        public const string AzureApiAccountKeyHeader = "Ocp-Apim-Subscription-Key";

        public const string ChitChatUrlFormat = "https://www.bing.com/search?q={0}&p1=%5BChitChat%20UserId=%22310%22%5D";

        public const string CortanaUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Trident/7.0; Touch; rv:11.0; Cortana 1.6.1.52; 10.0.0.0.10586.21) like Gecko";

        public const string BingDataApiBaseUrl = "https://www.bingapis.com/";

        public const string BingDataApiUrl = "api/v5/search?q={0}&appid={1}&setmkt=en-us";

        public const string BingDataApiAppID = "FA15B041D8404F934D4866D461005AE5159F0A90";

        public const string LUISUrlFormat = "https://api.projectoxford.ai/luis/v1/application?id=30aa83c1-7760-4a7d-84db-fd9a31a451ed&subscription-key=af79ebce73804e53b12f797a6cfc3909&q={0}";

        public const string LUISUrlNew = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/2ae8f4bb-bdef-4cee-aaeb-d70b7931cdf1?subscription-key=69cca90cd10c4d12917bfd509b51f6f8&verbose=true&timezoneOffset=0&q={0}";

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