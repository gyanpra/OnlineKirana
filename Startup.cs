using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineKirana.Startup))]
namespace OnlineKirana
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
