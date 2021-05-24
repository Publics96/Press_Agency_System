using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string PostTitle { get; set; }

        [Required]
        [MaxLength(3000)]
        public string PostBody { get; set; }

        public DateTime PostDate { get; set; }

        [Required]
        public string PostType { get; set; }

        public string ImagePath { get; set; }

        public PostState State { get; set; }

        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
    public enum PostState
    {
        Waiting = 1,
        Accepted = 2,
        Rejected = 3
    }
}