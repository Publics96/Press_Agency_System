using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Press_Agency_System.Models;

namespace Press_Agency_System.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult AllUsers()
        {
            var posts = db.Users.Include(x => x.Roles).ToList();

            return Json(new { data = posts }, JsonRequestBehavior.AllowGet);

        }


        //[Authorize]
        public ActionResult AllPosts()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var posts = db.Posts.Include(x=> x.User);
            return Json(new { data = posts }, JsonRequestBehavior.AllowGet);

        }
        //[Authorize]
        public ActionResult AllPostsForEditor(string UserId)
        {
            var posts = db.Posts.Where(e => e.UserId == UserId).ToList();
            return Json(new { data = posts }, JsonRequestBehavior.AllowGet);

        }

        // GET: /Post/
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.User);
            return View(posts.ToList());
        }

        // GET: /Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: /Post/Create
        public ActionResult Create()
        {
            ViewBag.PostCategories = PostCategories.AllCategories;
            return View();
        }

        // POST: /Post/Create
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase UploadedImage)
        {
            ViewBag.PostCategories = PostCategories.AllCategories;
            if (ModelState.IsValid)
            {
                if (UploadedImage != null)
                {
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/Posts Images"), UploadedImage.FileName);
                    UploadedImage.SaveAs(path);
                    post.ImagePath = UploadedImage.FileName;
                }
                else
                {
                    post.ImagePath = "DefaultImageForPost.jpg";
                }

                post.UserId = User.Identity.GetUserId();
                post.IsActive = true;
                post.State = PostState.Waiting;

                post.PostDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: /Post/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PostCategories = PostCategories.AllCategories;
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // POST: /Post/Edit/5
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase UploadedImage)
        {
            ViewBag.PostCategories = PostCategories.AllCategories;
            if (ModelState.IsValid)
            {
                if (UploadedImage != null)
                {
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/Posts Images"), UploadedImage.FileName);
                    UploadedImage.SaveAs(path);
                    post.ImagePath = UploadedImage.FileName;
                }



                post.UserId = User.Identity.GetUserId();
                post.IsActive = true;
                post.State = 0;
                post.State = PostState.Waiting;
                post.PostDate = DateTime.Now;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }


        // GET: /Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

 

        public ActionResult Like(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var react = new InteractedPosts();
            react.PostId = post.Id;
            react.UserId = User.Identity.GetUserId();
            react.Like = 1;
            db.IneractedPosts.Add(react);
            db.SaveChanges();

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
