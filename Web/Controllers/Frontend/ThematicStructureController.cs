using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend;

public class ThematicStructureController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IQueryEntityRepository<RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository;
    private readonly IQueryEntityRepository<Section, System.Guid> sectionRepository;
    private readonly IQueryEntityRepository<Epigraph, System.Guid> epigraphRepository;
    private readonly IQueryEntityRepository<Theme, System.Guid> themeRepository;
    private readonly IQueryEntityRepository<Map, string> mapRepository;

    public ThematicStructureController(
        IQueryEntityRepository<Map, string> mapRepository, 
        IQueryEntityRepository<Theme, System.Guid> themeRepository, 
        IQueryEntityRepository<Epigraph, System.Guid> epigraphRepository, 
        IQueryEntityRepository<Section, System.Guid> sectionRepository, 
        IQueryEntityRepository<RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository)
    {
        this.richTextBlockRepository = richTextBlockRepository;
        this.sectionRepository = sectionRepository;
        this.epigraphRepository = epigraphRepository;
        this.themeRepository = themeRepository;
        this.mapRepository = mapRepository;
    }

    public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.ThematicStructureIndexViewModel> Index()
    {
        var sections = sectionRepository.GetAll().OrderBy(entity => entity.Priority).ToArray();

        var sectionEpigraphs = epigraphRepository.GetAll().GroupBy(entity => entity.Section).ToArray();

        var model = new Models.View.Frontend.ThematicStructureIndexViewModel
        {
            Intro = richTextBlockRepository.Get(Camaguey.Persistence.Enumerations.RichTextBlockKind.PageThematicStructureIntro).Content,
            Structure = []
        };

        foreach (var item in sections)
        {
            var section = new SectionDisplayViewModel();

            section.Import(item);

            var sectionEpigraphModels = new List<EpigraphDisplayViewModel>();

            var ephigraphThemesDictionary = new Dictionary<EpigraphDisplayViewModel, Dictionary<ThemeDisplayViewModel, Models.View.Frontend.MapIndexViewModel[]>>();

            var epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == item.Id).OrderBy(_model => _model.Priority).ToArray();

            foreach (var _item in epigraphs)
            {
                var epigraph = new EpigraphDisplayViewModel();

                epigraph.Import(_item);

                var themeMapDictionary = new Dictionary<ThemeDisplayViewModel, Models.View.Frontend.MapIndexViewModel[]>();

                var themes = themeRepository.FindBy(entity => entity.EpigraphId == _item.Id).OrderBy(_model => _model.Priority).ToArray();

                foreach (var __item in themes)
                {
                    var theme = new ThemeDisplayViewModel();

                    theme.Import(__item);

                    var maps = new List<Models.View.Frontend.MapIndexViewModel>();

                    foreach (var bridge in __item.MapBridges)
                    {
                        var entity = mapRepository.Get(bridge.SternId);

                        var _model = new Models.View.Frontend.MapIndexViewModel { Current = new MapDisplayViewModel() };

                        _model.Current.Import(entity);

                        _model.Graphs = entity.Images.Where(entity => entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Chart).Select(bridge =>
                        {
                            var _model = new ImageDisplayViewModel();

                            _model.Import(bridge.Image);

                            return _model;
                        }).ToArray();
                        _model.Pictures = entity.Images.Where(entity => entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Picture).Select(bridge =>
                        {
                            var _model = new ImageDisplayViewModel();

                            _model.Import(bridge.Image);

                            return _model;
                        }).ToArray();
                        _model.Videos = entity.Videos.Select(bridge =>
                        {
                            var _model = new VideoDisplayViewModel();

                            _model.Import(bridge.Video);

                            return _model;
                        }).ToArray();
                        _model.Documents = entity.Documents.Select(bridge =>
                        {
                            var _model = new DocumentDisplayViewModel();

                            _model.Import(bridge.Document);

                            return _model;
                        }).ToArray();

                        maps.Add(_model);
                    }

                    themeMapDictionary.Add(theme, maps.OrderBy(_model => _model.Current.Priority).ToArray());
                }

                ephigraphThemesDictionary.Add(epigraph, themeMapDictionary);
            }

            model.Structure.Add(section, ephigraphThemesDictionary);
        }

        return View(model);
    }
}