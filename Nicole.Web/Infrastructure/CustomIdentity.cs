using Nicole.Library.Models;
using System.Security.Principal;

namespace Nicole.Web.Infrastructure
{
    public class CustomIdentity : IIdentity
    {
        private readonly Account _user;

        public CustomIdentity(Account user)
        {
            _user = user;
        }

        public Account User
        {
            get { return _user; }
        }

        public string Name
        {
            get
            {
                return _user == null ? "" : _user.Employee.Name;
            }
        }
        

        public string AuthenticationType
        {
            get
            {
                return "Custom";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _user != null;
            }
        }
    }
}