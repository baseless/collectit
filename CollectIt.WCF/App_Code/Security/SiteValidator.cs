using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;

namespace CollectIt.WCF.Security
{
    public class SiteValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            //throw new SecurityTokenException("Authentication failed");
        }
    }
}