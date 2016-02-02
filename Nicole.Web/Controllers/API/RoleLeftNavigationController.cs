using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class RoleLeftNavigationController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleLeftNavigationController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<Role, RoleModel>();
            Mapper.CreateMap<LeftNavigation, LeftNavigationModel>();
            var model =
                _roleService.GetRoles()
                    .SelectMany(n => n.LeftNavigations, (role, navigation) => new { role, navigation })
                    .OrderBy(n => n.role.Id)
                    .ThenBy(n => n.navigation.Id).ToArray()
                    .Select(n => new RoleLeftNavigationModel
                    {
                        LeftNavigationModel = Mapper.Map<LeftNavigation, LeftNavigationModel>(n.navigation),
                        RoleModel = Mapper.Map<Role, RoleModel>(n.role)
                    }).ToArray();
            return model;
        }
        public object Post(RoleLeftNavigationModel model)
        {
            if (model == null || model.LeftNavigationModel == null || model.RoleModel == null)
            {
                return Failed("不得为空");
            }
            var role = _roleService.GetRole(model.RoleModel.Id);
            var leftNavigation = _roleService.GetLeftNavigation(model.LeftNavigationModel.Id);
            if (role != null && leftNavigation != null)
            {
                role.LeftNavigations.Add(leftNavigation);
                if (leftNavigation.Parent != null)
                {
                    role.LeftNavigations.Add(leftNavigation.Parent);
                }
                try
                {
                    _roleService.Update();
                    return Success();
                }
                catch (Exception ex)
                {
                    return Failed(ex.Message);
                }
            }
            return Failed("找不到人员或者菜单");
        }

        public object Delete(RoleLeftNavigationModel model)
        {
            var role = _roleService.GetRole(model.RoleModel.Id);
            var leftNavigation = _roleService.GetLeftNavigation(model.LeftNavigationModel.Id);
            if (role != null && leftNavigation != null)
            {
                if (leftNavigation.Parent == null)
                {
                    var subNavgation = _roleService.GetLeftNavigations().Where(n => n.Parent.Id == leftNavigation.Id).ToArray();
                    foreach (var item in subNavgation)
                    {
                        role.LeftNavigations.Remove(item);
                    }
                }
                role.LeftNavigations.Remove(leftNavigation);
                if (role.LeftNavigations.Count <= 1)
                {
                    role.LeftNavigations.Clear();
                }
                try
                {
                    _roleService.Update();
                    return Success();
                }
                catch (Exception ex)
                {
                    return Failed(ex.Message);
                }
            }
            return Failed("找不到人员或者菜单");
        }
    }
}
