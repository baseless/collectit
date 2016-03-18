using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CollectIt.Web.ChannelService;
using CollectIt.Web.ItemService;
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
        private readonly ItemServiceClient _itemClient;

        public MemberController()
        {
            _channelClient = new ChannelServiceClient();
            _subscriptionClient = new SubscriptionServiceClient();
            _itemClient = new ItemServiceClient();
        }

        public async Task<ActionResult> Index()
        {
            var model = new MemberIndexViewModel {Items = await _itemClient.LatestAsync(User.Identity.Name)};
            return View(model);
        }

        public ActionResult Search()
        {var model = new MemberSearchViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Search(MemberSearchViewModel model)
        {
            model.Items = await _itemClient.QueryAsync(User.Identity.Name, model.Filters);
            model.Filters = "";
            return View(model);
        }

        public ActionResult Manage()
        {
            var channels = _channelClient.All(User.Identity.Name);
            return View(new MemberManageViewModel {Channels = channels});
        }

        public ActionResult Subscribe(string partition, string row)
        {
            var channel = _channelClient.Get(User.Identity.Name, partition, row);
            if (channel == null)
                return HttpNotFound("Channel not found!");

            var subscription = _subscriptionClient.Get(User.Identity.Name, partition, row);
            var model = new SubscribeViewModel {Name = channel.Name};

            if (subscription != null)
                model.Filters = string.Join(",", subscription.Filters);

            return View(model);
        }

        [HttpPost, ActionName("Subscribe")]
        public async Task<ActionResult> SubscribePost(SubscribeViewModel model, string partition, string row)
        {
            await _subscriptionClient.SubscribeAsync(User.Identity.Name, partition, row, model.Filters);
            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Unsubscribe(string partition, string row)
        {
            await _subscriptionClient.UnsubscribeAsync(User.Identity.Name, partition, row);
            return RedirectToAction("Manage");
        }
    }
}