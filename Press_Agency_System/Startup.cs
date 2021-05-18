using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Press_Agency_System.Models;

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

        }
        private void CreationOfDefaultRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));



            if (!roleManager.RoleExists("Admin"))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = "Admin";
                roleManager.Create(AdminRole);

                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";

                var flag = userManager.Create(user, "admin1234");
                if (flag.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Editor"))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = "Editor";
                roleManager.Create(AdminRole);
            }
            if (!roleManager.RoleExists("Viewer"))
            {
                IdentityRole AdminRole = new IdentityRole();
                AdminRole.Name = "Viewer";
                roleManager.Create(AdminRole);
            }

        }
    }
}
