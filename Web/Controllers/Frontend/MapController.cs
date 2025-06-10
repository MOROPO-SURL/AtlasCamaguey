using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class MapController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Map, string> mapRepository;
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public MapController(Microsoft.Extensions.Configuration.IConfiguration configuration, IEntityRepository<Section, System.Guid> sectionRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository, IEntityRepository<Map, string> mapRepository)
        {
            this.mapRepository = mapRepository;
            this.epigraphRepository = epigraphRepository;
            this.sectionRepository = sectionRepository;
            this.configuration = configuration;
        }

        [Microsoft.AspNetCore.Mvc.Route("{key}/epigraph/{epigraphId}")]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.MapIndexViewModel> Index(string key, System.Guid epigraphId)
        {
            var map = mapRepository.Get(key);

            var model = new Models.View.Frontend.MapIndexViewModel
            {
                QgisServerBaseUri = configuration["QGIS:ServerBaseUri"],
                QgisProjectsRootPath = configuration["QGIS:ProjectsPath"],
                Current = new Models.View.MapDisplayViewModel(),
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
                }).ToArray(),
                Epigraph = new Models.View.EpigraphDisplayViewModel(),
                Section = new Models.View.SectionDisplayViewModel(),
                Sections = sectionRepository.GetAll().OrderBy(entity => entity.Priority).ToArray().Select(entity =>
                {
                    var _model = new Models.View.SectionDisplayViewModel();

                    _model.Import(entity);

                    _model.Epigraphs = epigraphRepository.FindBy(_entity => _entity.SectionId == entity.Id).OrderBy(_entity => _entity.Priority).ToArray().Select(_entity =>
                    {
                        var __model = new Models.View.EpigraphDisplayViewModel();

                        __model.Import(_entity);

                        return __model;
                    }).ToArray();

                    return _model;
                }).ToArray()
            };

            model.Current.Import(map);

            var epigraph = epigraphRepository.Get(epigraphId);

            model.Epigraph.Import(epigraph);

            var section = sectionRepository.Get(epigraph.SectionId);

            model.Section.Import(section);
            model.Section.Epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == section.Id).OrderBy(entity => entity.Priority).ToArray().Select(entity =>
            {
                var _model = new Models.View.EpigraphDisplayViewModel();

                _model.Import(entity);

                return _model;
            }).ToArray();

            return View(model);
        }
    }
}
