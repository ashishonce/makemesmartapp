using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using makemesmarter.Features;
using makemesmarter.Helpers;

namespace makemesmarter.Models
{
    public class CommentFeatures
    {
        public Category category;

        public Status status;

        public Filetype fileType;

        public CommentLength commentLength;

        public Upvotes numUpVotes;

        public ThreadLength threadLength;

        public Sentiment sentiment;

    }
}