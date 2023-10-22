using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class EpigraphController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;

        public EpigraphController(IEntityRepository<Epigraph, System.Guid> epigraphRepository)
        {
            this.epigraphRepository = epigraphRepository;
        }

        [Microsoft.AspNetCore.Mvc.Route("{key}")]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.EpigraphIndexViewModel> Index(System.Guid key)
        {
            var epigraph = epigraphRepository.Get(key);

            var model = new Models.View.Frontend.EpigraphIndexViewModel
            {
                Epigraph = new Models.View.EpigraphDisplayViewModel(),
                Maps = epigraph.MapBridges.Select(bridge =>
                {
                    var _model = new Models.View.MapDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Images = epigraph.ImageBridges.Select(bridge =>
                {
                    var _model = new Models.View.ImageDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Videos = epigraph.VideoBridges.Select(bridge =>
                {
                    var _model = new Models.View.VideoDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                Documents = epigraph.DocumentBridges.Select(bridge =>
                {
                    var _model = new Models.View.DocumentDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray(),
                KeyWords = epigraph.KeyWordBridges.Select(bridge =>
                {
                    var _model = new Models.View.KeyWordDisplayViewModel();

                    _model.Import(bridge.Stern);

                    return _model;
                }).ToArray()
            };

            model.Epigraph.Import(epigraph);

            return View(model);
        }
    }
}
