using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Nicole.Web.Startup))]
namespace Nicole.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
