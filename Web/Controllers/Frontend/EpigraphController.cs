using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class EpigraphController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;

        public EpigraphController(IEntityRepository<Section, System.Guid> sectionRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository)
        {
            this.epigraphRepository = epigraphRepository;
            this.sectionRepository = sectionRepository;
        }

        [Microsoft.AspNetCore.Mvc.Route("{key}")]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.EpigraphIndexViewModel> Index(System.Guid key)
        {
            var epigraph = epigraphRepository.Get(key);

            var model = new Models.View.Frontend.EpigraphIndexViewModel
            {
                Current = new Models.View.EpigraphDisplayViewModel(),
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
                }).ToArray(),
                Section = new Models.View.SectionDisplayViewModel(),
                Sections = sectionRepository.GetAll().ToArray().Select(entity =>
                {
                    var _model = new Models.View.SectionDisplayViewModel();

                    _model.Import(entity);

                    _model.Epigraphs = epigraphRepository.FindBy(_entity => _entity.SectionId == entity.Id).ToArray().Select(_entity =>
                    {
                        var __model = new Models.View.EpigraphDisplayViewModel();

                        __model.Import(_entity);

                        return __model;
                    }).ToArray();

                    return _model;
                }).ToArray()
            };

            model.Current.Import(epigraph);

            var section = sectionRepository.Get(epigraph.SectionId.Value);

            model.Section.Import(section);
            model.Section.Epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == section.Id).ToArray().Select(entity =>
            {
                var _model = new Models.View.EpigraphDisplayViewModel();

                _model.Import(entity);

                return _model;
            }).ToArray();

            return View(model);
        }
    }
}
