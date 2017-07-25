using makemesmarter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Helpers
{
    public static class CommentValueAnalyser
    {
        public static  bool IsValuable(CommentCategory category, CommentStatus status, int commentLength, int numUpVotes, int threadLength)
        {
            var weight = 0;
            weight += CommentAnalyser.NumUpVotesWeight(numUpVotes);
            weight += CommentAnalyser.StatusWeight(status);
            //weight += CommentAnalyser.CodeChangeWeight(IsCodeChange);
            weight += CommentAnalyser.CategoryWeight(category);
            weight += CommentAnalyser.ThreadLengthWeight(threadLength);

            var weightedAverage = weight / 5;
            if (weightedAverage >= 30 && weightedAverage <= 54)
            {
                return true;
            }

            return false;
        }

    }
}