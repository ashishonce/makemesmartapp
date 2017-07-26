using makemesmarter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace makemesmarter.Helpers
{
    public static class CommentValueAnalyser
    {
        public static  int IsValuable(string category, string status, int commentLength, int numUpVotes, int threadLength, List<Tuple<string, double>> statusWeights)
        {
            var weight = 0;
            CommentCategory commentCategory;
            CommentStatus commentStatus;
            Enum.TryParse(category, true, out commentCategory);
            Enum.TryParse(status, true, out commentStatus);

            var weighttuple = statusWeights.Find(x => x.Item1.Equals(commentStatus));
            var weightforStatus = 0.0;
            if (weighttuple != null)
            {
                weightforStatus = weighttuple.Item2;
             }
            else
            {
                weightforStatus = 0.5;
            }

            weight += CommentAnalyser.NumUpVotesWeight(numUpVotes)* 20;
            weight += CommentAnalyser.StatusWeight(commentStatus) * 15;

            //weight += CommentAnalyser.CodeChangeWeight(IsCodeChange);
            weight += CommentAnalyser.CategoryWeight(commentCategory) * 10;
            weight += CommentAnalyser.ThreadLengthWeight(threadLength) * 5;

            var weightedAverage = (weight * 100 )/ 385 ;
            return weightedAverage;
        }

    }
}