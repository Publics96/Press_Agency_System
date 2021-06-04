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
    [Authorize(Roles = "Admin,Editor,Viewer")]
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

            if (post == null )
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();

            if ((post.State == PostState.Waiting || post.State == PostState.Rejected) && !isAuthorized(userId))
            {
                return HttpNotFound();
            }

            
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
        [Authorize (Roles="Admin,Editor")]
        // GET: /Post/Create
        public ActionResult Create()
        {
            ViewBag.PostCategories = PostCategories.AllCategories;
            return View();
        }

        // POST: /Post/Create
        [Authorize(Roles = "Admin,Editor")]
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
        [Authorize(Roles = "Admin,Editor")]
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
        [Authorize(Roles = "Admin,Editor")]
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
        [Authorize(Roles = "Admin,Editor")]
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

            return RedirectToAction("Moderation");
        }
        [Authorize (Roles = "Admin,Editor,Viewer")]
        public ActionResult SavedPosts()
        {
            string userid = User.Identity.GetUserId();
            var savedposts = db.SavedPosts.Where(x=>x.UserId==userid).ToList().Select(x=>x.PostId);

            var posts = db.Posts.Include(x=>x.User).Where(x=>savedposts.Contains(x.Id)).ToList();

            var interactedposts = db.IneractedPosts.Where(x => savedposts.Contains(x.PostId)).ToList();

            Dictionary<int, int> views = new Dictionary<int, int>();
            Dictionary<int, int> likes = new Dictionary<int, int>();

            foreach (var i in posts)
            {
                views[i.Id] = interactedposts.Where(x=>x.PostId==i.Id).Count();
                likes[i.Id] = interactedposts.Where(x => x.PostId == i.Id).Where(x => x.Like == 1).Count();
            }
            SavedPostsViewModel model = new SavedPostsViewModel()
            {
                posts= posts,
                likes = likes,
                views = views
            };

            return View(model);
        }
        public bool isAuthorized(string userid)
        {
            var user = db.Users.Find(userid);
            if(user == null)
            {
                return false;
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string rolename = userManager.GetRoles(userid).FirstOrDefault();

            if(rolename=="Admin" || rolename == "Editor")
            {
                return true;

            }

            return false;
        }
    }
}
