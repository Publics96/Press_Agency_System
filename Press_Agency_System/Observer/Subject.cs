using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Press_Agency_System.Models;

namespace Press_Agency_System.Observer
{
    public class Subject : ISubject
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<IObserver> observers = new List<IObserver>();
        private Post post;
        public Subject(Post post)
        {
            this.post = post;
        }


        public void RegisterObserver(Observers observer)
        {
            db.observers.Add(observer);
            db.SaveChanges();
        }
        public void RemoveObserver(Observers observer)
        {
            db.observers.Remove(observer);
            db.SaveChanges();
        }
        public void NotifyObservers()
        {
            string message = post.PostTitle + " in " + post.PostType + " is out now!, check it out!.";
            
            var tablex = db.observers.ToList();
            if(tablex == null || tablex.Count()==0)
            {
                return;
            }

            foreach (var i in tablex)
            {
                i.update(message);
            }
        }
    }
}