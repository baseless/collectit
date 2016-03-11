using CollectIt.Web.Models;
using Owin;

namespace CollectIt.Web
{
    public partial class Startup
    {
        public void ConfigureInjection(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(CollectItDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            //Potential site injections are placed here!
        }
    }
}