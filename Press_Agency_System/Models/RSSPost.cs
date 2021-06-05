using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class RSSPost
    {
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string publishingDate { get; set; }

    }
}