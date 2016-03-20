using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.Web.SubscriptionService
{
    public partial class SubscriptionServiceClient
    {
        public static SubscriptionServiceClient Create()
        {
            var service = new SubscriptionServiceClient();
            var creds = new ClientCredentials
            {
                UserName =
                {
                    UserName = RoleEnvironment.GetConfigurationSettingValue("WCFClientUser"),
                    Password = RoleEnvironment.GetConfigurationSettingValue("WCFClientPassword")
                },
                ServiceCertificate =
                {
                    Authentication =
                    {
                        CertificateValidationMode = X509CertificateValidationMode.None,
                        RevocationMode = X509RevocationMode.NoCheck
                    }
                }
            };

            var defaultCredentials = service.Endpoint.Behaviors.Find<ClientCredentials>();
            service.Endpoint.Behaviors.Remove(defaultCredentials);
            service.Endpoint.Behaviors.Add(creds);
            return service;
        }
    }
}