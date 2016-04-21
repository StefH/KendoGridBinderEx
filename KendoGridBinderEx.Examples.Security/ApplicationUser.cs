using KendoGridBinderEx.Examples.Business.Entities;
using System;
using System.Security.Principal;

namespace KendoGridBinderEx.Examples.Security
{
    public class ApplicationUser : IPrincipal
    {
        private readonly User _user;

        public ApplicationUser(User user)
        {
            _user = user;
            Identity = new ApplicationIdentity(user.IdentityName, true);
        }

        public IIdentity Identity { get; }

        public bool IsInRole(string role)
        {
            return _user.HasRole(role);
        }
    }

    public class ApplicationIdentity : IIdentity
    {
        public ApplicationIdentity(string name, bool authenticated)
        {
            Name = name;
            IsAuthenticated = authenticated;
        }

        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated { get; }

        public string Name { get; }
    }
}