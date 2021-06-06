using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Press_Agency_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Press_Agency_System.Controllers
{

    public class UserController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        static public List<String> RolesName = new List<string>(new string[] { "Admin", "Editor", "Viewer" });




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
        // GET: User
        //public ActionResult index()
        //{

        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        public ActionResult AllUsers()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var allusers = db.Users.Include(e => e.roleType).Where(x => x.activeUser == true).ToList();
            return Json(new { data = allusers }, JsonRequestBehavior.AllowGet);

        }



        public ActionResult AddUser(ApplicationUser userObj, string userrole, HttpPostedFileBase UploadedImage)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (ModelState.IsValid)
            {
                if (UploadedImage != null)
                {
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/Persons Images"), UploadedImage.FileName);
                    UploadedImage.SaveAs(path);
                    userObj.PhotoPath = UploadedImage.FileName;
                }
                else
                {
                    userObj.PhotoPath = "defaultuserphoto.jpg";
                }
                userObj.activeUser = true;
                userObj.roleType = db.Roles.FirstOrDefault(x => x.Name == userrole);

                var flag = userManager.Create(userObj, userObj.PasswordHash);
                if (flag.Succeeded)
                {
                    if (userrole == "Admin")
                    {
                        userManager.AddToRole(userObj.Id, Roles.Admin);
                        userObj.roleType.Name = "Admin";

                    }
                    else if (userrole == "Editor")
                    {
                        userManager.AddToRole(userObj.Id, Roles.Editor);
                        userObj.roleType.Name = "Editor";
                    }
                    else if (userrole == "Viewer")
                    {
                        userManager.AddToRole(userObj.Id, Roles.Viewer);
                        userObj.roleType.Name = "Viewer";
                    }
                    TempData["mess"] = "user added correctly";
                    return RedirectToAction("Index");
                }

                TempData["mess"] = "invaild username or password";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["mess"] = "invaild username or password";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult index(string id)
        {
            if (id == null)
            {
                return View();
            }

            ApplicationUser obj = db.Users.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }

            return View(obj);
        }
        [Authorize(Roles = "Admin,Editor,Viewer")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser obj = db.Users.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }

            //  ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", obj.UserId);
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor,Viewer")]
        public ActionResult Edit([Bind(Include = "Id,UserName,FirstName,LastName,Email,Phone,PhotoPath,roleType,activeUser")] ApplicationUser obj, HttpPostedFileBase UploadedImage, string userrole)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (ModelState.IsValid)
            {

                if (UploadedImage != null)
                {
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/Persons Images"), UploadedImage.FileName);
                    UploadedImage.SaveAs(path);
                    obj.PhotoPath = UploadedImage.FileName;

                }
                else
                {
                    obj.PhotoPath = "defaultuserphoto.jpg";
                }
                obj.activeUser = true;
                obj.roleType = db.Roles.FirstOrDefault(x => x.Name == userrole);
                var dotuse = db.Users.Include(x => x.roleType).FirstOrDefault(e => e.Id == obj.Id);
                userManager.RemoveFromRole(dotuse.Id, dotuse.roleType.Name);
                if (userrole == "Admin")
                {
                    userManager.AddToRole(obj.Id, Roles.Admin);
                    obj.roleType.Name = "Admin";

                }
                else if (userrole == "Editor")
                {
                    userManager.AddToRole(obj.Id, Roles.Editor);
                    obj.roleType.Name = "Editor";
                }
                else if (userrole == "Viewer")
                {
                    userManager.AddToRole(obj.Id, Roles.Viewer);
                    obj.roleType.Name = "Viewer";
                }
                updatfunc(obj);
                return RedirectToAction("Index");
            }


            return View(obj);
        }

        private void updatfunc(ApplicationUser obj)
        {
            ApplicationUser existing = db.Users.Find(obj.Id);

            ((IObjectContextAdapter)db).ObjectContext.Detach(existing);


            db.Entry(obj).State = EntityState.Modified;

            db.SaveChanges();
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser obj = db.Users.Find(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser obj = db.Users.FirstOrDefault(x => x.Id == id);
            obj.activeUser = false;

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

//[HttpPost, ActionName("Delete")]
//public ActionResult DeleteConfirmed(string id)
//{
//    ApplicationUser obj = db.Users.Find(id);
//    db.Users.Remove(obj);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}