using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CollectIt.Web.Startup))]
namespace CollectIt.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
