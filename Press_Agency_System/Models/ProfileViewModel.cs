using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class ProfileViewModel
    {
        public int viewcount;
        public int likecount;
        public List<Post> UserPosts;
        public ApplicationUser user;
        
    }
    
}