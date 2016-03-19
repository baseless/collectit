using System.Reflection;
using System.Web.Mvc;

namespace CollectIt.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}