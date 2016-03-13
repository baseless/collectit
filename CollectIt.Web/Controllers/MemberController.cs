using System.Web.Mvc;
using CollectIt.Web.ChannelService;
using CollectIt.Web.Models.ViewModels;
using CollectIt.Web.SubscriptionService;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CollectIt.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly ChannelServiceClient _channelClient;
        private readonly SubscriptionServiceClient _subscriptionClient;

        public MemberController()
        {
            _channelClient = new ChannelServiceClient();
            _subscriptionClient = new SubscriptionServiceClient();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            var channels = _channelClient.All(User.Identity.Name);
            return View(new MemberManageViewModel {Channels = channels});
        }

        public ActionResult Subscribe(string partition, string row)
        {
            var channel = _channelClient.Get(User.Identity.Name, partition, row);
            if(channel == null)
                return HttpNotFound("Channel not found!");

            var subscription = _subscriptionClient.Get(User.Identity.Name, partition, row);
            var model = new SubscribeViewModel {Name = channel.Name};
            
            if (subscription != null)
                model.Filters = string.Join(",", subscription.Filters);
            
            return View(model);
        }

        public ActionResult Unsubscribe()
        {
            
            return View();
        }
    }
}