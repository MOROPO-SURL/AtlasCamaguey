using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class MapController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Map, System.Guid> mapRepository;

        public MapController(IEntityRepository<Map, System.Guid> mapRepository)
        {
            this.mapRepository = mapRepository;
        }

        [Microsoft.AspNetCore.Mvc.Route("{key}")]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.MapIndexViewModel> Index(System.Guid key)
        {
            var map = mapRepository.Get(key);

            var model = new Models.View.Frontend.MapIndexViewModel
            {
                Map = new Models.View.MapDisplayViewModel(),
                Authors = map.AuthorBridges.Select(bridge =>
                {
                    var _model = new Models.View.AuthorDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Images = map.ImageBridges.Select(bridge =>
                {
                    var _model = new Models.View.ImageDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Videos = map.VideoBridges.Select(bridge =>
                {
                    var _model = new Models.View.VideoDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Documents = map.DocumentBridges.Select(bridge =>
                {
                    var _model = new Models.View.DocumentDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                KeyWords = map.KeyWordBridges.Select(bridge =>
                {
                    var _model = new Models.View.KeyWordDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray()
            };

            model.Map.Import(map);

            return View(model);
        }
    }
}
