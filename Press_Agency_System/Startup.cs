using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Press_Agency_System.Models;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(Press_Agency_System.Startup))]
namespace Press_Agency_System
{
    public partial class Startup
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreationOfDefaultRoles();
            app.MapSignalR();
        }
        public void AddUsers()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            ApplicationUser user = new ApplicationUser();
            user.UserName = "Viewer";
            user.FirstName = "Hamada";
            user.LastName = "Ahmed";
            user.Email = "mohamed@gmail.com";

            var flag = userManager.Create(user, "Viewer1234");
            if (flag.Succeeded)
            {
                userManager.AddToRole(user.Id, Roles.Viewer);
            }
            ApplicationUser user3 = new ApplicationUser();
            user3.UserName = "Admin2";
            user3.FirstName = "Hamada";
            user3.LastName = "Ahmed";
            user3.Email = "mohamed@gmail.com";

            var flag3 = userManager.Create(user3, "Admin1234");
            if (flag3.Succeeded)
            {
                userManager.AddToRole(user3.Id, Roles.Admin);
            }

            ApplicationUser user2 = new ApplicationUser();
            user2.UserName = "Editor";
            user2.FirstName = "Hamada";
            user2.LastName = "Ahmed";
            user2.Email = "mohamed@gmail.com";

            var flag2 = userManager.Create(user2, "Editor1234");
            if (flag2.Succeeded)
            {
                userManager.AddToRole(user2.Id, Roles.Editor);
            }


        }
        private void CreationOfDefaultRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));



            if (!roleManager.RoleExists(Roles.Admin))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = Roles.Admin;
                roleManager.Create(AdminRole);

                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.FirstName = "Mohamed";
                user.LastName = "Ahmed";
                user.Email = "mohamed@gmail.com";
                user.roleType = _db.Roles.FirstOrDefault(x=>x.Name == "Admin");
                user.PhotoPath = "person1.png";
                user.Phone = "01006879078";
                user.activeUser = true;
                

                var flag = userManager.Create(user, "admin1234");
                if (flag.Succeeded)
                {
                    userManager.AddToRole(user.Id, Roles.Admin);
                }
            }
            if (!roleManager.RoleExists(Roles.Editor))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = Roles.Editor;
                roleManager.Create(AdminRole);
            }
            if (!roleManager.RoleExists(Roles.Viewer))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = Roles.Viewer;
                roleManager.Create(AdminRole);
            }

        }
    }
}
