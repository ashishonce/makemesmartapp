using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace makemesmarter.Models
{
    public class CommentThread
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CommentId { set; get; }

        public string JoinedComments { get; set; }

        public string Status { get; set; }

        public int ThreadCount { get; set; }

        public int IsUseful { get; set; }

        public int CumlativeLikes { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        public double SentimentValue { get; set; }

        public string CommentInitiator { get; set; }

        public int initiatorCommentLength { get; set; }

        public string PrAuthorId
        {
            get; set;
        }

        public int PullRequestId
        {
            get; set;
        }

        public List<string> KeyPrases { get; set; }

    }
}