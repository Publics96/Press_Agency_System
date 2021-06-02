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
            var posts = db.Posts.Include(x => x.User);
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
            Post post = db.Posts.Include(e => e.User).SingleOrDefault(x => x.Id == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            List<InteractedPosts> allposts = db.IneractedPosts.Where(e => e.PostId == id).ToList();
            var obj = allposts.Find(e => e.UserId == userId) as InteractedPosts;
            byte likedbyuser;
            if (obj == null)
                likedbyuser = 0;
            else
                likedbyuser = obj.Like;


            var objsaved = db.SavedPosts.FirstOrDefault(e => e.PostId == id && e.UserId == userId) as SavedPosts;
            var savedbyuser = false;
            if (objsaved != null)
            {
                savedbyuser = true;
            }

            PostDetailsViewModel pdvm = new PostDetailsViewModel()
            {
                allInteractions = allposts,
                Post = post,
                likedbyuser = likedbyuser,
                saveedbyuser = savedbyuser
            };

            return View(pdvm);
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

        public ActionResult ViewPost(int? id)
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
            react.Like = 0;
            var interactedpost = db.IneractedPosts.FirstOrDefault(x => x.UserId == react.UserId && x.PostId == react.PostId) as InteractedPosts;
            if (interactedpost != null)
            {
                return Content("0");
            }
            else
            {
                db.IneractedPosts.Add(react);
                db.SaveChanges();
                return Content("1");
            }

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
            var interactedpost = db.IneractedPosts.FirstOrDefault(x => x.UserId == react.UserId && x.PostId == react.PostId);
            if (interactedpost != null)
            {
                if (interactedpost.Like == 2)
                {
                    interactedpost.Like = 1;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("1");
                }
                else if (interactedpost.Like == 0)
                {
                    interactedpost.Like = 1;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("1");
                }
                else
                {
                    interactedpost.Like = 0;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("-1");
                }

            }
            else
            {
                db.IneractedPosts.Add(react);
                db.SaveChanges();
                return Content("1");
            }

        }
        public ActionResult disLike(int? id)
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
            var interactedpost = db.IneractedPosts.FirstOrDefault(x => x.UserId == react.UserId && x.PostId == react.PostId);
            if (interactedpost != null)
            {
                if (interactedpost.Like == 2)
                {
                    interactedpost.Like = 0;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("-1");
                }
                else if (interactedpost.Like == 0)
                {
                    interactedpost.Like = 2;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("1");
                }
                else
                {
                    interactedpost.Like = 2;
                    db.Entry(interactedpost).State = EntityState.Modified;
                    db.SaveChanges();
                    return Content("1");
                }

            }
            else
            {
                db.IneractedPosts.Add(react);
                db.SaveChanges();
                return Content("1");
            }

        }
        public ActionResult Save(int id)
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
            var savedPost = new SavedPosts();
            savedPost.PostId = id;
            savedPost.UserId = User.Identity.GetUserId();
            var savedPostInDb = db.SavedPosts.FirstOrDefault(x => x.UserId == savedPost.UserId && x.PostId == savedPost.PostId) as SavedPosts;
            if (savedPostInDb == null)
            {
                db.SavedPosts.Add(savedPost);
                db.SaveChanges();
                return Content("Saved");
            }
            else
            {
                db.SavedPosts.Remove(savedPostInDb);
                db.SaveChanges();
                return Content("UnSaved");
            }


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Moderation()
        {
            if (User == null)
            {
                return RedirectToAction("Login");
            }
            List<Post> posts = db.Posts.Include(x => x.User).Where(x => x.State <= PostState.Waiting).ToList();


            return View(posts);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ModerationAccept(int id)
        {
            if (User == null)
            {
                return RedirectToAction("Login");
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.State = PostState.Accepted;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();

            //List<Post> posts = db.Posts.Where(y => y.State <= (int)PostState.Waiting).ToList();
            //ModerationViewModel viewModel = new ModerationViewModel();
            //viewModel.posts = posts;
            return RedirectToAction("Moderation");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ModerationReject(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.State = PostState.Rejected;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();

            //List<Post> posts = db.Posts.Where(y => y.State <= (int)PostState.Waiting).ToList();
            //ModerationViewModel viewModel = new ModerationViewModel();
            //viewModel.posts = posts;
            return RedirectToAction("Moderation");
        }
    }
}
