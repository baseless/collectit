using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
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
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _itemClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _itemClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _itemClient.Endpoint.Behaviors.Add(creds);

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
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _itemClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _itemClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _itemClient.Endpoint.Behaviors.Add(creds);

            model.Items = await _itemClient.QueryAsync(User.Identity.Name, model.Filters);
            model.Filters = "";
            return View(model);
        }

        public ActionResult Manage()
        {
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _channelClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _channelClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _channelClient.Endpoint.Behaviors.Add(creds);

            var channels = _channelClient.All(User.Identity.Name);
            return View(new MemberManageViewModel {Channels = channels});
        }

        public ActionResult Subscribe(string partition, string row)
        {
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _channelClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _channelClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _channelClient.Endpoint.Behaviors.Add(creds);

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
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _subscriptionClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _subscriptionClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _subscriptionClient.Endpoint.Behaviors.Add(creds);

            await _subscriptionClient.SubscribeAsync(User.Identity.Name, partition, row, model.Filters);
            return RedirectToAction("Manage");
        }

        public async Task<ActionResult> Unsubscribe(string partition, string row)
        {
            var creds = new ClientCredentials
            {
                UserName = { UserName = "user", Password = "pass" },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = _subscriptionClient.Endpoint.Behaviors.Find<ClientCredentials>();
            _subscriptionClient.Endpoint.Behaviors.Remove(defaultCredentials);
            _subscriptionClient.Endpoint.Behaviors.Add(creds);

            await _subscriptionClient.UnsubscribeAsync(User.Identity.Name, partition, row);
            return RedirectToAction("Manage");
        }
    }
}