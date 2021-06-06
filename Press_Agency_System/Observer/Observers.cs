using Press_Agency_System.Models;
using Press_Agency_System.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.Observer
{
    public class Observers : IObserver
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Observers(string userId, ISubject subject)
        {
            this.UserId = userId;
            subject.RegisterObserver(this);
        }
        public Observers(string userId)
        {
            this.UserId = UserId;
        }
        public Observers()
        {

        }

        public void update(string message)
        {
            new AppServices().SendMessage(UserId, message);
        }
    }
}