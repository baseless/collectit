using System.Web.Mvc;
using CollectIt.Web.ChannelService;
using CollectIt.Web.Models.ViewModels;

namespace CollectIt.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ChannelServiceClient _channelClient;

        public MemberController()
        {
            _channelClient = new ChannelServiceClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            var channels = _channelClient.AllChannels(User.Identity.Name);
            return View(new MemberManageViewModel { Channels = channels });
        }
    }
}