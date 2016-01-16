using System.Security.Principal;

namespace Nicole.Web.Infrastructure
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly CustomIdentity _identity;

        public CustomPrincipal(CustomIdentity identity)
        {
            this._identity = identity;
        }

        public IIdentity Identity
        {
            get
            {
                return this._identity;
            }
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}