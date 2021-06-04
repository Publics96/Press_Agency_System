using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class SavedPostsViewModel
    {
        public List<Post> posts;
        public Dictionary<int, int> views;
        public Dictionary<int, int> likes;

    }
}