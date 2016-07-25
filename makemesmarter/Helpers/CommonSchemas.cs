using System.Collections.Generic;

namespace makemesmarter.Helpers
{
    public class CommonSchemas
    {
        public class EntitiesApiResult
        {
            public string _type { get; set; }

            public EntitiesResult entities { get; set; }
        }

        public class EntitiesResult
        {
            public string readLink { get; set; }

            public string queryScenario { get; set; }

            public IList<EntityResultValue> value { get; set; }
        }

        public class EntityResultValue
        {
            public string id { get; set; }

            public string readLink { get; set; }

            public string name { get; set; }

            public CommonSchemas.Image image { get; set; }

            public string description { get; set; }

            public string webSearchUrl { get; set; }

            public EntityPresentationInfo entityPresentationInfo { get; set; }

            public AggregatedRating aggregateRating { get; set; }
        }

        public class AggregatedRating
        {
            public double ratingValue { get; set; }

            public double bestRating { get; set; }

            public int ratingCount { get; set; }
        }

        public class EntityPresentationInfo
        {
            public string entityScenario { get; set; }

            public IList<string> entityTypeHints { get; set; }

            public IList<FormattedFact> formattedFacts { get; set; }

            public string entityTypeDisplayHint { get; set; }
        }

        public class FormattedFact
        {
            public string label { get; set; }

            public IList<Item> items { get; set; }
        }

        public class Item
        {
            public string _type { get; set; }

            public string text { get; set; }
        }

        public class Image
        {
            public Thumbnail thumbnail { get; set; }
        }

        public class Thumbnail
        {
            public string contentUrl { get; set; }

            public int width { get; set; }

            public int height { get; set; }
        }
    }
}