using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace makemesmarter.Models
{
    public class ReviewScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ReviewerID { get; set; }

        public string FileType { get; set; }

        public double Score { get; set; }
    }
}