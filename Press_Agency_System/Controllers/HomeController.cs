using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Press_Agency_System.Models;

namespace Press_Agency_System.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            var savedposts = db.SavedPosts.Where(x => x.UserId == userid).ToList().Select(x => x.PostId);

            var posts = db.Posts.Include(x=>x.User).Where(x => savedposts.Contains(x.Id)).ToList();

            var interactedposts = db.IneractedPosts.Where(x => savedposts.Contains(x.PostId)).ToList();

            Dictionary<int, int> views = new Dictionary<int, int>();
            Dictionary<int, int> likes = new Dictionary<int, int>();

            foreach (var i in posts)
            {
                views[i.Id] = interactedposts.Where(x => x.PostId == i.Id).Count();
                likes[i.Id] = interactedposts.Where(x => x.PostId == i.Id).Where(x => x.Like == 1).Count();
            }
            SavedPostsViewModel model = new SavedPostsViewModel()
            {
                posts = posts,
                likes = likes,
                views = views
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}