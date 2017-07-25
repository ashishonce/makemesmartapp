using makemesmarter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Helpers
{
    public static class CommentValueAnalyser
    {
        public static  bool IsValuable(string category, string status, int commentLength, int numUpVotes, int threadLength)
        {
            var weight = 0;
            CommentCategory commentCategory;
            CommentStatus commentStatus;
            Enum.TryParse(category, out commentCategory);
            Enum.TryParse(category, out commentStatus);
            weight += CommentAnalyser.NumUpVotesWeight(numUpVotes);
            weight += CommentAnalyser.StatusWeight(commentStatus);
            //weight += CommentAnalyser.CodeChangeWeight(IsCodeChange);
            weight += CommentAnalyser.CategoryWeight(commentCategory);
            weight += CommentAnalyser.ThreadLengthWeight(threadLength);

            var weightedAverage = weight / 3;
            if (weightedAverage >= 25)
            {
                return true;
            }

            return false;
        }

    }
}