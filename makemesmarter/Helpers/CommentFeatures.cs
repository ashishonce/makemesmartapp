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
        public Category category { get; set; }

        public Status status { get; set; }

        public CommentLength commentLength { get; set; }

        public Upvotes numUpVotes { get; set; }

        public ThreadLength threadLength { get; set; }

        public CodeChange codeChange { get; set; }
    }
}