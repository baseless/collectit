using System.Diagnostics;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace CollectIt.WCF
{
    public class SiteValidator : UserNamePasswordValidator
    {
        /// <summary>
        /// Simple validation method which checks against a hard-coded username and password atm.
        /// </summary>
        /// <param name="userName">The username sent by client</param>
        /// <param name="password">The password sent by client</param>
        public override void Validate(string userName, string password)
        {
            Debug.WriteLine("WCF request recieved: Username: " + userName + ", Password: " + password);
            if (!userName.Equals("CollectItSite") || !password.Equals("7xYX4ITMYsVk9pvJaWAzDBjF8c5J8JeedSC5ol6z1LKvVvvkH9"))
                throw new FaultException("WCF authentication credentials invalid");
        }
    }
}