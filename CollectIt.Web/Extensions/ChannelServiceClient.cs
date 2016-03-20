using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.Web.ChannelService
{
    public partial class ChannelServiceClient
    {
        public static ChannelServiceClient Create()
        {
            var channelService = new ChannelServiceClient();
            var creds = new ClientCredentials
            {
                UserName = { UserName = RoleEnvironment.GetConfigurationSettingValue("WCFClientUser"), Password = RoleEnvironment.GetConfigurationSettingValue("WCFClientPassword") },
                ServiceCertificate = { Authentication = { CertificateValidationMode = X509CertificateValidationMode.None, RevocationMode = X509RevocationMode.NoCheck } }
            };

            var defaultCredentials = channelService.Endpoint.Behaviors.Find<ClientCredentials>();
            channelService.Endpoint.Behaviors.Remove(defaultCredentials);
            channelService.Endpoint.Behaviors.Add(creds);
            return channelService;
        }
    }
}