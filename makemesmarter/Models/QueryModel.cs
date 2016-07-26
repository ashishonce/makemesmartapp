using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace makemesmarter.Models
{
    public class QueryModel
    {
        [Key]
        public int QueryId { get; set; }

        public string Query { get; set; }

       public string Reply { get; set; }
    }
}