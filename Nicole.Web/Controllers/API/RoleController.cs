using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Nicole.Library.Models;
using Nicole.Library.Services;
using Nicole.Web.Models;

namespace Nicole.Web.Controllers.API
{
    public class RoleController : BaseApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public object Get([FromUri] RoleModel key)
        {
            
            Mapper.CreateMap<Role, RoleModel>();
            var result = _roleService.GetRoles();
            return result.Select(Mapper.Map<Role, RoleModel>);
        }
    }
}