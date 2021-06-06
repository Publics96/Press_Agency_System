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
        public ActionResult Index2()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var posts = db.Posts.Include(e => e.User).Where(x => x.State == PostState.Accepted).ToList();
            var interactedposts = db.IneractedPosts.ToList();

            return Json(new { posts = posts, interactedposts = interactedposts }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Filter(string input)
        {
            if (input != null)
            {
                db.Configuration.ProxyCreationEnabled = false;
                var posts = db.Posts.Include(e => e.User).Where(x => x.State == PostState.Accepted).ToList();
                var interactedposts = db.IneractedPosts.ToList();
                var list = posts.Where(e => e.PostType.ToLower().Contains(input) || e.PostTitle.ToLower().Contains(input) || e.User.FirstName.ToLower().Contains(input) || e.User.LastName.ToLower().Contains(input)).ToList();
                var matched = list.Select(e => e.Id).ToList();
                if (matched.Count > 0)
                {
                    return Json(new { posts = list, interactedposts = interactedposts }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Content("false");
                }
            }
            else
                return Content("false");
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