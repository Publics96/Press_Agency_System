using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Press_Agency_System.Startup))]
namespace Press_Agency_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
