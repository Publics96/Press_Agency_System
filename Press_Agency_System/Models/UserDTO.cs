﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Models
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public bool IsOnline { get; set; }

    }
}