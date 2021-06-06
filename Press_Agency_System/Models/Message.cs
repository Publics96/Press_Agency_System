using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser FromUser { get; set; }
        public ApplicationUser ToUser { get; set; }

        public string Message1 { get; set; }

        public DateTime Date { get; set; }
    }

    public class UserConnection
    {
        public int Id { get; set; }
        public ApplicationUser UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}