using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class SavedPosts
    {
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public Post Post { get; set; }
        public int PostId { get; set; }

    }
}