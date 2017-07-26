using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using makemesmarter.Features;
using makemesmarter.Models;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using makemesmarter.Models;
using Newtonsoft.Json;

namespace makemesmarter.Helpers
{
    public class CommentAnalyser
    {
        public static int NumUpVotesWeight(int numUpVotes)
        {
            if (numUpVotes > 1)
            {
                return 9;
            }
            else if (numUpVotes == 1)
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }

        public static int StatusWeight(CommentStatus status)
        {
            if (status == CommentStatus.Fixed || status == CommentStatus.Closed)
            {
                return 6;
            }
            else if (status == CommentStatus.Pending)
            {
                return 4;
            }
            else if (status == CommentStatus.Active)
            {
                return 2;
            }
            else if (status == CommentStatus.WontFix)
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public static int CodeChangeWeight(bool isCodeChange)
        {
            if (isCodeChange)
            {
                return 50;
            }
            else
            {
                return 0;
            }
        }

        public static int CategoryWeight(CommentCategory category)
        {
            if (category == CommentCategory.Defect)
            {
                return 9;
            }
            else if (category == CommentCategory.Design)
            {
                return 8;
            }
            else if (category == CommentCategory.Logical)
            {
                return 7;
            }
            else if (category == CommentCategory.SolutionApproach || category == CommentCategory.APICalls)
            {
                return 4;
            }
            else if (category == CommentCategory.Localization)
            {
                return 3;
            }
            else if (category == CommentCategory.Style)
            {
                return 2;
            }
            else if (category == CommentCategory.CodeHygiene || category == CommentCategory.Formatting || category == CommentCategory.Documentation)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int ThreadLengthWeight(int threadLength)
        {
            if (threadLength > 4)
            {
                return 5;
            }
            else if (threadLength > 1 && threadLength < 4)
            {
                return threadLength;
            }

            return 0;
        }

        public static string GetLUISCategory(string queryString, IList<string> entities)
        {
            try
            {
                // Call Luis to get the intent and entities for a given query
                var luisUrl = string.Format(Constants.LUISUrlNew, queryString);
                string intent = string.Empty;
                using (var wc = new WebDownload())
                {
                    var json = wc.DownloadString(luisUrl);
                    JObject jsonObject = JObject.Parse(json);

                    intent = (jsonObject["intents"][0])["intent"].ToString();
                }

                return intent;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}