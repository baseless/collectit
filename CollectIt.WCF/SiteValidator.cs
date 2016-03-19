using System.Diagnostics;
using System.IdentityModel.Selectors;

namespace CollectIt.WCF
{
    public class SiteValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            Debug.WriteLine("WCF request recieved: Username: " + userName + ", Password: " + password);
        }
    }
}