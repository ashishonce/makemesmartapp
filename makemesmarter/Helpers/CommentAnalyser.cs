using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using makemesmarter.Features;
using makemesmarter.Models;

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
                return -60;
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
                return -40;
            }
            else
            {
                return 0;
            }
        }

        public static int ThreadLengthWeight(int threadLength)
        {
            if (threadLength > 10)
            {
                return 30;
            }
            else if (threadLength > 0 && threadLength < 10)
            {
                return threadLength + 20;
            }
            else
            {
                return 0;
            }
        }
    }
}