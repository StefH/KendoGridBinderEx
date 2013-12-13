using KendoGridBinderEx.Examples.Business.Entities;
using System;
using System.Security.Principal;

namespace KendoGridBinderEx.Examples.Security
{
    public class ApplicationUser : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly User _user;

        public ApplicationUser(User user)
        {
            _user = user;
            _identity = new ApplicationIdentity(user.IdentityName, true);
        }

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            return _user.HasRole(role);
        }
    }

    public class ApplicationIdentity : IIdentity
    {
        private readonly string _name;
        private readonly bool _authenticated;

        public ApplicationIdentity(string name, bool authenticated)
        {
            _name = name;
            _authenticated = authenticated;
        }

        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _authenticated;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}