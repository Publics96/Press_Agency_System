using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class PostDetailsViewModel
    {
        public List<InteractedPosts> allInteractions;

        public Post Post { get; set; }

        public byte likedbyuser { get; set; }

        public bool saveedbyuser { get; set; }
    }
}