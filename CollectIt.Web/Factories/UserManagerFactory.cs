using System.Web;
using CollectIt.Web.Interfaces;
using Microsoft.AspNet.Identity.Owin;

namespace CollectIt.Web.Factories
{
    public class UserManagerFactory : IManagerFactory<ApplicationUserManager>
    {
        private readonly ApplicationUserManager _manager;

        public UserManagerFactory() {}

        public UserManagerFactory(ApplicationUserManager manager) { _manager = manager; }

        public static IManagerFactory<ApplicationUserManager> Create() { return new UserManagerFactory(); }

        public ApplicationUserManager Manager => _manager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

        public void Dispose()
        {
            _manager?.Dispose();
        }
    }
}