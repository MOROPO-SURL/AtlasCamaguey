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
        private readonly IEntityRepository<Camaguey.Persistence.Entities.Theme, System.Guid> themeRepository;

        public ThematicStructureController(IEntityRepository<Camaguey.Persistence.Entities.Theme, System.Guid> themeRepository, IEntityRepository<Camaguey.Persistence.Entities.Epigraph, System.Guid> epigraphRepository, IEntityRepository<Camaguey.Persistence.Entities.Section, System.Guid> sectionRepository, IEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository)
        {
            this.richTextBlockRepository = richTextBlockRepository;
            this.sectionRepository = sectionRepository;
            this.epigraphRepository = epigraphRepository;
            this.themeRepository = themeRepository;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.ThematicStructureIndexViewModel> Index()
        {
            var sections = sectionRepository.GetAll().OrderBy(entity => entity.Priority).ToArray();

            var sectionEpigraphs = epigraphRepository.GetAll().GroupBy(entity => entity.Section).ToArray();

            var model = new Models.View.Frontend.ThematicStructureIndexViewModel
            {
                Intro = richTextBlockRepository.Get(Camaguey.Persistence.Enumerations.RichTextBlockKind.PageThematicStructureIntro).Content,
                Structure = new Dictionary<SectionDisplayViewModel, Dictionary<EpigraphDisplayViewModel, Dictionary<ThemeDisplayViewModel, MapDisplayViewModel[]>>>()
            };

            foreach (var item in sections)
            {
                var section = new SectionDisplayViewModel();

                section.Import(item);

                var sectionEpigraphModels = new List<EpigraphDisplayViewModel>();

                var ephigraphThemesDictionary = new Dictionary<EpigraphDisplayViewModel, Dictionary<ThemeDisplayViewModel, MapDisplayViewModel[]>>();

                var epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == item.Id).OrderBy(_model => _model.Priority).ToArray();

                foreach (var _item in epigraphs)
                {
                    var epigraph = new EpigraphDisplayViewModel();

                    epigraph.Import(_item);

                    var themeMapDictionary = new Dictionary<ThemeDisplayViewModel, MapDisplayViewModel[]>();

                    var themes = themeRepository.FindBy(entity => entity.EpigraphId == _item.Id).OrderBy(_model => _model.Priority).ToArray();

                    foreach (var __item in themes)
                    {
                        var theme = new ThemeDisplayViewModel();

                        theme.Import(__item);

                        var maps = new List<MapDisplayViewModel>();

                        foreach (var bridge in __item.MapBridges)
                        {
                            var mapModel = new MapDisplayViewModel();

                            mapModel.Import(bridge.Stern);

                            maps.Add(mapModel);
                        }

                        themeMapDictionary.Add(theme, maps.OrderBy(_model => _model.Priority).ToArray());
                    }

                    ephigraphThemesDictionary.Add(epigraph, themeMapDictionary);
                }

                model.Structure.Add(section, ephigraphThemesDictionary);
            }

            return View(model);
        }
    }
}
