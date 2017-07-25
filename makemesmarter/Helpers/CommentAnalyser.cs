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
            if (numUpVotes > 10)
            {
                return 70;
            }
            else if (numUpVotes > 0 && numUpVotes < 10)
            {
                return numUpVotes + 60;
            }
            else
            {
                return 0;
            }
        }

        public static int StatusWeight(CommentStatus status)
        {
            if (status == CommentStatus.Fixed)
            {
                return 60;
            }
            else if (status == CommentStatus.Resolved)
            {
                return 58;
            }
            else if (status == CommentStatus.Pending)
            {
                return 56;
            }
            else if (status == CommentStatus.Active)
            {
                return 54;
            }
            else if (status == CommentStatus.WontFix)
            {
                return -20;
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
                return 40;
            }
            else if (category == CommentCategory.Design)
            {
                return 39;
            }
            else if (category == CommentCategory.Logical)
            {
                return 38;
            }
            else if (category == CommentCategory.SolutionApproach)
            {
                return 37;
            }
            else if (category == CommentCategory.Localization)
            {
                return 36;
            }
            else if (category == CommentCategory.Style)
            {
                return 35;
            }
            else if (category == CommentCategory.CodeHygiene)
            {
                return 34;
            }
            else if (category == CommentCategory.None)
            {
                return 10;
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
                return 30;
            }
            else if (threadLength > 0 && threadLength < 4)
            {
                return threadLength + 20;
            }
            else
            {
                return 0;
            }
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