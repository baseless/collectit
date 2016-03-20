using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Web;
using CollectIt.Web.SubscriptionService;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.Web.StatusService
{
    public partial class StatusServiceClient
    {
        public static StatusServiceClient Create()
        {
            var service = new StatusServiceClient();
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