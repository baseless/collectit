using System.Diagnostics;
using System.Reflection;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.WCF
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            var ass = Assembly.GetAssembly(typeof(SiteValidator));
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!ASSEMBLY:" + ass.GetName());
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
