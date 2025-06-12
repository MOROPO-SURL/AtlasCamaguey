using Microsoft.EntityFrameworkCore;
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
        private readonly IEntityRepository<Theme, System.Guid> themeRepository;

        public SearchController(IEntityRepository<Theme, System.Guid> themeRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository, IEntityRepository<Section, System.Guid> sectionRepository, Microsoft.Extensions.Localization.IStringLocalizer<SearchController> localizer, ILogger<SearchController> logger)
            : base(localizer, logger)
        {
            this.sectionRepository = sectionRepository;
            this.epigraphRepository = epigraphRepository;
            this.themeRepository = themeRepository;
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
                var sectionEntities = sectionRepository.GetAll().ToArray();

                model.Sections = sectionEntities.Where(entity => entity.Name.Contains(key, System.StringComparison.OrdinalIgnoreCase) || entity.Description.Contains(key, System.StringComparison.OrdinalIgnoreCase)).ToArray().Select(entity =>
                {
                    var _model = new SectionDisplayViewModel();

                    _model.Import(entity);

                    return _model;
                }).ToArray();
            }

            var epigraphEntities = epigraphRepository.GetAll().ToArray();

            var epigraphModels = new List<EpigraphDisplayViewModel>();

            var mapModels = new List<MapDisplayViewModel>();

            foreach (var entity in epigraphEntities)
            {
                if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Epigraph)
                {
                    if (entity.Name.Contains(key, System.StringComparison.OrdinalIgnoreCase) || entity.Description.Contains(key, System.StringComparison.OrdinalIgnoreCase))
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
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key, System.StringComparison.OrdinalIgnoreCase))
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
                        if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key, System.StringComparison.OrdinalIgnoreCase))
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
                        if (!string.IsNullOrEmpty(bridge.Stern.Description) && bridge.Stern.Description.Contains(key, System.StringComparison.OrdinalIgnoreCase))
                        {
                            var __model = new DocumentDisplayViewModel();

                            __model.Import(bridge.Stern);

                            documentModels.Add(__model);
                        }
                    }
                }

                var themeEntities = themeRepository.FindBy(_entity => _entity.EpigraphId == entity.Id).ToArray();

                var mapEnties = new List<Map>();

                foreach (var theme in themeEntities)
                {
                    mapEnties.AddRange(theme.MapBridges.Select(bridge => bridge.Stern).ToArray());
                }

                foreach (var _entity in mapEnties)
                {
                    if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Map)
                    {
                        if (_entity.Name.Contains(key, System.StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(_entity.Description) && _entity.Description.Contains(key, System.StringComparison.OrdinalIgnoreCase)))
                        {
                            var _model = new MapDisplayViewModel();

                            _model.Import(_entity);

                            _model.EphigraphId = entity.Id;

                            mapModels.Add(_model);
                        }
                    }

                    if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Image)
                    {
                        foreach (var bridge in _entity.ImageBridges)
                        {
                            if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key, System.StringComparison.OrdinalIgnoreCase))
                            {
                                var __model = new ImageDisplayViewModel();

                                __model.Import(bridge.Stern);

                                imageModels.Add(__model);
                            }
                        }
                    }

                    if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Video)
                    {
                        foreach (var bridge in _entity.VideoBridges)
                        {
                            if (!string.IsNullOrEmpty(bridge.Stern.About) && bridge.Stern.About.Contains(key, System.StringComparison.OrdinalIgnoreCase))
                            {
                                var __model = new VideoDisplayViewModel();

                                __model.Import(bridge.Stern);

                                videoModels.Add(__model);
                            }
                        }
                    }

                    if (informationResourceKind == null || informationResourceKind.Value == Enumerations.InformationResourceKind.Document)
                    {
                        foreach (var bridge in _entity.DocumentBridges)
                        {
                            if (!string.IsNullOrEmpty(bridge.Stern.Description) && bridge.Stern.Description.Contains(key, System.StringComparison.OrdinalIgnoreCase))
                            {
                                var __model = new DocumentDisplayViewModel();

                                __model.Import(bridge.Stern);

                                documentModels.Add(__model);
                            }
                        }
                    }
                }
            }

            if (epigraphModels.Any()) model.Epigraphs = epigraphModels.ToArray();            

            if (mapModels.Any()) model.Maps = mapModels.ToArray();

            if (imageModels.Any()) model.Images = imageModels.ToArray();

            if (videoModels.Any()) model.Videos = videoModels.ToArray();

            if (documentModels.Any()) model.Documents = documentModels.ToArray();

            return View(model);
        }
    }
}
