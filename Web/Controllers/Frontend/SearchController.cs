using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    public class SearchController : Framework.Guama.Core.Presentation.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;
        private readonly IEntityRepository<Map, string> mapRepository;

        public SearchController(IEntityRepository<Map, string> mapRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository, IEntityRepository<Section, System.Guid> sectionRepository, Microsoft.Extensions.Localization.IStringLocalizer<SearchController> localizer, ILogger<SearchController> logger)
            : base(localizer, logger)
        {
            this.sectionRepository = sectionRepository;
            this.epigraphRepository = epigraphRepository;
            this.mapRepository = mapRepository;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.SearchResultViewModel> Result(string key, Enumerations.InformationResourceKind? informationResourceKind)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                SetDefaultNotification(Framework.Guama.Core.Presentation.Models.View.NotificationType.Warning, "Realizar Búsqueda", "Es obligatorio especificar el parámetro de búsqueda para obtener resultados.", null);

                return RedirectToAction("Index", "Home");
            }

            var model = new Models.View.Frontend.SearchResultViewModel(key, informationResourceKind);

            var imageModels = new List<ImageDisplayViewModel>();
            var videoModels = new List<VideoDisplayViewModel>();
            var documentModels = new List<DocumentDisplayViewModel>();

            if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Section)
            {
                model.Sections = sectionRepository.FindBy(entity => entity.Name.Contains(key) || entity.Description.Contains(key)).ToArray().Select(entity =>
                {
                    var _model = new SectionDisplayViewModel();

                    _model.Import(entity);

                    return _model;
                }).ToArray();
            }

            var epigraphEntities = epigraphRepository.GetAll().ToArray();

            var epigraphModels = new List<EpigraphDisplayViewModel>();

            foreach (var entity in epigraphEntities)
            {
                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Epigraph)
                {
                    if (entity.Name.Contains(key) || entity.Description.Contains(key))
                    {
                        var _model = new EpigraphDisplayViewModel();

                        _model.Import(entity);

                        epigraphModels.Add(_model);
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Image)
                {
                    foreach (var bridge in entity.ImageBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key))
                        {
                            var __model = new ImageDisplayViewModel();

                            __model.Import(bridge.Stern);

                            imageModels.Add(__model);
                        }
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Video)
                {
                    foreach (var bridge in entity.VideoBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key))
                        {
                            var __model = new VideoDisplayViewModel();

                            __model.Import(bridge.Stern);

                            videoModels.Add(__model);
                        }
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Document)
                {
                    foreach (var bridge in entity.DocumentBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.Description) && bridge.Stern.Description.Contains(key))
                        {
                            var __model = new DocumentDisplayViewModel();

                            __model.Import(bridge.Stern);

                            documentModels.Add(__model);
                        }
                    }
                }
            }

            if (epigraphModels.Any()) model.Epigraphs = epigraphModels.ToArray();

            var mapEntities = mapRepository.GetAll().ToArray();

            var mapModels = new List<MapDisplayViewModel>();

            foreach (var entity in mapEntities)
            {
                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Map)
                {
                    if (entity.Name.Contains(key) || entity.Description.Contains(key))
                    {
                        var _model = new MapDisplayViewModel();

                        _model.Import(entity);

                        mapModels.Add(_model);
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Image)
                {
                    foreach (var bridge in entity.ImageBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key))
                        {
                            var __model = new ImageDisplayViewModel();

                            __model.Import(bridge.Stern);

                            imageModels.Add(__model);
                        }
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Video)
                {
                    foreach (var bridge in entity.VideoBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key))
                        {
                            var __model = new VideoDisplayViewModel();

                            __model.Import(bridge.Stern);

                            videoModels.Add(__model);
                        }
                    }
                }

                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Document)
                {
                    foreach (var bridge in entity.DocumentBridges)
                    {
                        if (!string.IsNullOrEmpty(bridge.Stern.Description) && bridge.Stern.Description.Contains(key))
                        {
                            var __model = new DocumentDisplayViewModel();

                            __model.Import(bridge.Stern);

                            documentModels.Add(__model);
                        }
                    }
                }
            }

            if (mapModels.Any()) model.Maps = mapModels.ToArray();

            if (imageModels.Any()) model.Images = imageModels.ToArray();

            if (videoModels.Any()) model.Videos = videoModels.ToArray();

            if (documentModels.Any()) model.Documents = documentModels.ToArray();

            return View(model);
        }
    }
}
