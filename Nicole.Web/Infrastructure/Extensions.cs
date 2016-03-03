using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using Nicole.Web.Models;
using Nicole.Library.Models;
using Nicole.Service.Services;
using Nicole.Service;

namespace Nicole.Web.Infrastructure
{
    public static class Extensions
    {
        public static UserModel GetUser(this IIdentity identity)
        {

            try
            {
                var customIdentity = identity as CustomIdentity;
                Account user = null;
                if (customIdentity == null)
                {
                    var _identity = identity as ClaimsIdentity;

                    if (_identity != null)
                    {
                        var userId = _identity.Claims.Where(n => n.Type == "Id").Select(n => n.Value).FirstOrDefault();
                        if (userId != null)
                        {
                            user = new AccountService(new NicoleDataContext()).GetAccount(new Guid(userId));
                        }
                    }
                }
                else
                {
                    user = customIdentity.User;
                }
                Mapper.CreateMap<Account, UserModel>().ForMember(n=>n.AccountId,opt=>opt.MapFrom(src=>src.Id))
                    .ForMember(n => n.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id));
                return Mapper.Map<Account, UserModel>(user);
            }
            catch (Exception)
            {
                //this.GetLogger().Error(ex.Message);
            }
            return null;
        }

        public static string Shorten(this String title)
        {
            if (title.Length > 12)
            {
                return title.Substring(0, 10) + "...";
            }
            else
            {
                return title;
            }
        }
    }
}
