using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class ChatBoxModel
    {
        public ApplicationUser ToUser { get; set; }
        public List<MessageDTO> Messages { get; set; }
        public List<Message> MessageContent { get; set; }
    }
}