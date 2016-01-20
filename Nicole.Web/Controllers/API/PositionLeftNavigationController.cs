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
    public class PositionLeftNavigationController : BaseApiController
    {
        private readonly IPositionService _positionService;

        public PositionLeftNavigationController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        public object Get()
        {
            Mapper.Reset();
            Mapper.CreateMap<Position, PositionModel>();
            Mapper.CreateMap<LeftNavigation, LeftNavigationModel>();
            var model =
                _positionService.GetPositions()
                    .SelectMany(n => n.LeftNavigations, (position, navigation) => new { position, navigation })
                    .OrderBy(n => n.position.Id)
                    .ThenBy(n => n.navigation.Id).ToArray()
                    .Select(n => new PositionLeftNavigationModel
                    {
                        LeftNavigationModel = Mapper.Map<LeftNavigation, LeftNavigationModel>(n.navigation),
                        PositionModel = Mapper.Map<Position, PositionModel>(n.position)
                    }).ToArray();
            return model;
        }
        public object Post(PositionLeftNavigationModel model)
        {
            if (model == null || model.LeftNavigationModel == null || model.PositionModel == null)
            {
                return Failed("不得为空");
            }
            var position = _positionService.GetPosition(model.PositionModel.Id);
            var leftNavigation = _positionService.GetLeftNavigation(model.LeftNavigationModel.Id);
            if (position != null && leftNavigation != null)
            {
                position.LeftNavigations.Add(leftNavigation);
                if (leftNavigation.Parent != null)
                {
                    position.LeftNavigations.Add(leftNavigation.Parent);
                }
                try
                {
                    _positionService.Update();
                    return Success();
                }
                catch (Exception ex)
                {
                    return Failed(ex.Message);
                }
            }
            return Failed("找不到人员或者菜单");
        }

        public object Delete(PositionLeftNavigationModel model)
        {
            var position = _positionService.GetPosition(model.PositionModel.Id);
            var leftNavigation = _positionService.GetLeftNavigation(model.LeftNavigationModel.Id);
            if (position != null && leftNavigation != null)
            {
                if (leftNavigation.Parent == null)
                {
                    var subNavgation = _positionService.GetLeftNavigations().Where(n => n.Parent.Id == leftNavigation.Id).ToArray();
                    foreach (var item in subNavgation)
                    {
                        position.LeftNavigations.Remove(item);
                    }
                }
                position.LeftNavigations.Remove(leftNavigation);
                if (position.LeftNavigations.Count <= 1)
                {
                    position.LeftNavigations.Clear();
                }
                try
                {
                    _positionService.Update();
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
