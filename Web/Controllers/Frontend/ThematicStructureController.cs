using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    public class ThematicStructureController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository;
        private readonly IEntityRepository<Camaguey.Persistence.Entities.Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<Camaguey.Persistence.Entities.Epigraph, System.Guid> epigraphRepository;

        public ThematicStructureController(IEntityRepository<Camaguey.Persistence.Entities.Epigraph, System.Guid> epigraphRepository, IEntityRepository<Camaguey.Persistence.Entities.Section, System.Guid> sectionRepository, IEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository)
        {
            this.richTextBlockRepository = richTextBlockRepository;
            this.sectionRepository = sectionRepository;
            this.epigraphRepository = epigraphRepository;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.ThematicStructureIndexViewModel> Index()
        {
            var sections = sectionRepository.GetAll().OrderBy(entity => entity.Priority).ToArray();

            var sectionEpigraphs = epigraphRepository.GetAll().GroupBy(entity => entity.Section).ToArray();

            var model = new Models.View.Frontend.ThematicStructureIndexViewModel
            {
                Intro = richTextBlockRepository.Get(Camaguey.Persistence.Enumerations.RichTextBlockKind.PageThematicStructureIntro).Content,
                Structure = new Dictionary<SectionDisplayViewModel, Dictionary<EpigraphDisplayViewModel, MapDisplayViewModel[]>>()
            };

            foreach (var section in sections)
            {
                var sectionModel = new SectionDisplayViewModel();

                sectionModel.Import(section);

                var sectionEpigraphModels = new List<EpigraphDisplayViewModel>();

                var epigraphMapDictionary = new Dictionary<EpigraphDisplayViewModel, MapDisplayViewModel[]>();

                var epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == section.Id).OrderBy(_model => _model.Priority).ToArray();

                foreach (var _item in epigraphs)
                {
                    var epigraphModel = new EpigraphDisplayViewModel();

                    epigraphModel.Import(_item);

                    var maps = new List<MapDisplayViewModel>();

                    foreach (var bridge in _item.MapBridges)
                    {
                        var mapModel = new MapDisplayViewModel();

                        mapModel.Import(bridge.Stern);

                        maps.Add(mapModel);
                    }

                    epigraphMapDictionary.Add(epigraphModel, maps.OrderBy(_model => _model.Priority).ToArray());
                }

                model.Structure.Add(sectionModel, epigraphMapDictionary);
            }

            return View(model);
        }
    }
}
