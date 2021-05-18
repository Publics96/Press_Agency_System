using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class Questions
    {
        public int Id { get; set; }

     
        public ApplicationUser ViwerUser { get; set; }

        public ApplicationUser EditorUser { get; set; }
        

        [MaxLength(1000)]
        [Required]
        public string MessageContent { get; set; }

        public DateTime Datetime { get; set; }


    }
}