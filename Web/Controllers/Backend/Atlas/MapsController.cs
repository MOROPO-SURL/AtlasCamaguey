using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Models.View;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class MapsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<Map, string, MapInputViewModel, MapDisplayViewModel>
    {
        private readonly ICommandEntityRepository<Image, System.Guid> commandImageRepository;
        private readonly ICommandEntityRepository<Document, System.Guid> commandDocumentRepository;
        private readonly ICommandEntityRepository<Video, System.Guid> commandVideoRepository;
        private readonly IQueryEntityRepository<KeyWord, System.Guid> queryKeyWordRepository;
        private readonly IQueryEntityRepository<Author, System.Guid> queryAuthorRepository;
        private readonly ICommandEntityRepository<Layer, System.Guid> commandLayerRepository;
        private readonly ICommandEntityRepository<MapImage, System.Guid> commandMapImageRepository;
        private readonly IQueryEntityRepository<MapImage, System.Guid> queryMapImageRepository;
        private readonly ICommandEntityRepository<MapVideo, System.Guid> commandMapVideoRepository;
        private readonly IQueryEntityRepository<MapVideo, System.Guid> queryMapVideoRepository;
        private readonly ICommandEntityRepository<MapDocument, System.Guid> commandMapDocumentRepository;
        private readonly IQueryEntityRepository<MapDocument, System.Guid> queryMapDocumentRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IDocumentFileManagerFacade documentFileManager;
        private readonly IVideoFileManagerFacade videoFileManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public MapsController(
            Microsoft.Extensions.Configuration.IConfiguration configuration, 
            IVideoFileManagerFacade videoFileManager, 
            IDocumentFileManagerFacade documentFileManager, 
            IImageFileManagerFacade imageFileManager, 
            IQueryEntityRepository<MapDocument, System.Guid> queryMapDocumentRepository, 
            ICommandEntityRepository<MapDocument, System.Guid> commandMapDocumentRepository, 
            IQueryEntityRepository<MapVideo, System.Guid> queryMapVideoRepository, 
            ICommandEntityRepository<MapVideo, System.Guid> commandMapVideoRepository, 
            IQueryEntityRepository<MapImage, System.Guid> queryMapImageRepository, 
            ICommandEntityRepository<MapImage, System.Guid> commandMapImageRepository, 
            ICommandEntityRepository<Layer, System.Guid> commandLayerRepository, 
            IQueryEntityRepository<Author, System.Guid> queryAuthorRepository, 
            IQueryEntityRepository<KeyWord, System.Guid> queryKeyWordRepository, 
            ICommandEntityRepository<Video, System.Guid> commandVideoRepository, 
            ICommandEntityRepository<Document, System.Guid> commandDocumentRepository, 
            ICommandEntityRepository<Image, System.Guid> commandImageRepository, 
            ICommandEntityRepository<Map, string> commandRepository, 
            IQueryEntityRepository<Map, string> queryRepository, 
            IStringLocalizer<MapsController> localizer, 
            ILogger<MapsController> logger) 
            : base(commandRepository, queryRepository, localizer, logger)
        {
            this.commandImageRepository = commandImageRepository;
            this.commandDocumentRepository = commandDocumentRepository;
            this.commandVideoRepository = commandVideoRepository;
            this.queryKeyWordRepository = queryKeyWordRepository;
            this.queryAuthorRepository = queryAuthorRepository;
            this.commandLayerRepository = commandLayerRepository;
            this.commandMapImageRepository = commandMapImageRepository;
            this.queryMapImageRepository = queryMapImageRepository;
            this.commandMapVideoRepository = commandMapVideoRepository;
            this.queryMapVideoRepository = queryMapVideoRepository;
            this.commandMapDocumentRepository = commandMapDocumentRepository;
            this.queryMapDocumentRepository = queryMapDocumentRepository;
            this.imageFileManager = imageFileManager;
            this.documentFileManager = documentFileManager;
            this.videoFileManager = videoFileManager;
            this.configuration = configuration;
        }

        [RequirePermissions(Constants.Permission.MapViewer)]
        public IActionResult Index()
        {
            return View();
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(MapInputViewModel inputViewModel)
        {
            return NotFound();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public override IActionResult Edit(string key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public override IActionResult Edit(MapInputViewModel model)
        {
            ModelState.ClearValidationState("ImageFile");
            ModelState.MarkFieldValid("ImageFile");

            if (Request.Form.Files.Count > 0)
            {
                foreach (var formFile in Request.Form.Files)
                {
                    switch (formFile.Name)
                    {
                        case "ImageFile":
                            model.ImageFile = formFile;

                            break;

                        case "MetadataFile":
                            model.MetadataFile = formFile;

                            break;
                    }
                }

                TryValidateModel(model);
            }

            return base.Edit(model);
        }

        [RequirePermissions(Constants.Permission.MapRemover)]
        public override IActionResult Delete(string key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.MapRemover)]
        public override IActionResult Delete(MapDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        [RequestSizeLimit(52428800)] // 50 mb
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapCreator)]
        public async System.Threading.Tasks.Task<IActionResult> UploadAsync(MapUploadViewModel model)
        {
            ModelState.ClearValidationState("MapArchive");

            model.MapArchive = Request.Form.Files[0];

            TryValidateModel(model);

            if (ModelState.IsValid)
            {
                var mapId = System.IO.Path.GetFileNameWithoutExtension(model.MapArchive.FileName).ToLowerInvariant();
                
                var qgisProjectsRootPath = configuration["QGIS:ProjectsPath"];

                var qgisProjectDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(qgisProjectsRootPath, mapId));

                try
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        model.MapArchive.CopyTo(memoryStream);

                        using (var uploadZipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false, System.Text.CodePagesEncodingProvider.Instance.GetEncoding(System.Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)))
                        {
                            uploadZipArchive.ExtractToDirectory(qgisProjectsRootPath, true);
                        }

                        if (!qgisProjectDirectoryInfo.Exists)
                        {
                            throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", "Estructura del empaqutado incorrecta!!!");
                        }

                        if (!System.IO.File.Exists(System.IO.Path.Combine(qgisProjectDirectoryInfo.FullName, string.Format("{0}.qgz", mapId))))
                        {
                            throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", "Archivo de proyecto no encontrado!!!");
                        }

                        using (var httpClient = new System.Net.Http.HttpClient())
                        {
                            var uri = string.Format("{0}?MAP={1}/{2}.qgz&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetCapabilities", configuration["QGIS:ServerBaseUri"], qgisProjectDirectoryInfo.FullName.Replace("\\", "/"), mapId);

                            var httpResponseMessage = await httpClient.GetAsync(uri);

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                                var projDescriptorXmlDocument = new System.Xml.XmlDocument();

                                projDescriptorXmlDocument.Load(contentStream);

                                var globalLayerNode = projDescriptorXmlDocument.ChildNodes[1].ChildNodes[1].ChildNodes[3];

                                var map = new Map
                                {
                                    Id = mapId,
                                    Scale = 10000
                                };

                                var globalLayerNodeChildNodesCount = globalLayerNode.ChildNodes.Count;

                                for (int i = 0; i < globalLayerNodeChildNodesCount; i++)
                                {
                                    var globalLayerNodeChild = globalLayerNode.ChildNodes[i];

                                    switch (globalLayerNodeChild.Name)
                                    {
                                        case "Title":
                                            map.Name = globalLayerNodeChild.InnerText;

                                            break;

                                        case "CRS":
                                            var crsValue = globalLayerNodeChild.InnerText;

                                            if (crsValue.StartsWith("CRS:"))
                                            {
                                                map.CRS = crsValue;
                                            }
                                            else
                                            {
                                                map.EPSG = crsValue;
                                            }

                                            break;

                                        case "EX_GeographicBoundingBox":
                                            map.WestBoundLongitude = decimal.Parse(globalLayerNodeChild.ChildNodes[0].InnerText.Replace(".", ","));
                                            map.EastBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[1].InnerText.Replace(".", ","));
                                            map.SouthBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[2].InnerText.Replace(".", ","));
                                            map.NorthBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[3].InnerText.Replace(".", ","));

                                            break;

                                        case "Layer":
                                            var childs = FindSubLayers(globalLayerNodeChild);

                                            if (childs.Count > 0)
                                            {
                                                foreach (var item in childs)
                                                {
                                                    map.Layers.Add(item.Value);
                                                }
                                            }
                                            else
                                            {
                                                var layer = new Layer
                                                {
                                                    Name = globalLayerNodeChild.ChildNodes[0].InnerText,
                                                    Title = globalLayerNodeChild.ChildNodes[1].InnerText,
                                                    Enabled = true,
                                                    Priority = 1,
                                                    PresentationStrategy = Camaguey.Persistence.Enumerations.LayerPresentationStrategy.WMS,
                                                    CRS = globalLayerNodeChild.ChildNodes[2].InnerText,
                                                    EPSG = globalLayerNodeChild.ChildNodes[3].InnerText,
                                                };

                                                map.Layers.Add(layer);
                                            }

                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(map.Name)) map.Name = map.Id;

                                commandRepository.Add(map, User.Identity.Name, true);
                            }
                            else
                            {
                                throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", string.Format("Archivo de proyecto no interpretable por el servidor de QGIS. Código del error: {0}!!!", httpResponseMessage.StatusCode));
                            }
                        }
                    }

                    SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                    return RedirectToAction("Edit", new { key = mapId });
                }
                catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                {
                    string message = $"Problemas cargando archivo de proyecto QGIS : {e.ErrorMessage}";
                    
                    logger.LogError(e, message);

                    SetDefaultNotification(NotificationType.Danger, message);

                    ModelState.AddModelError(e.ModelKey, e.ErrorMessage);

                    if (qgisProjectDirectoryInfo.Exists)
                    {
                        qgisProjectDirectoryInfo.Delete(true);
                    }
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, "Excepción no esperada cargando archivo de proyect QGIS!!!");

                    SetDefaultNotification(NotificationType.Danger, "Error no esperado cargando archivo de proyect QGIS!!!");

                    if (qgisProjectDirectoryInfo.Exists)
                    {
                        qgisProjectDirectoryInfo.Delete(true);
                    }
                }
            }
            else
            {
                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());
            }

            return RedirectToAction("Index");
        }

        [RequestSizeLimit(52428800)] // 50 mb
        [HttpPost]
        [Route("[action]/{key}")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public async System.Threading.Tasks.Task<IActionResult> ReplaceAsync(string key, MapUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fileMapId = System.IO.Path.GetFileNameWithoutExtension(model.MapArchive.FileName).ToLowerInvariant();

                if (key != fileMapId)
                {
                    SetDefaultNotification(NotificationType.Danger, "El archivo de proyecto QGIS especificado no coincide con la entrada de mapa actual.");

                    return RedirectToAction("Edit", new { key });
                }

                Map entity = commandRepository.Get(key);

                var qgisProjectsRootPath = configuration["QGIS:ProjectsPath"];

                string projectPath = System.IO.Path.Combine(qgisProjectsRootPath, key);

                var currentQgisProjectDirectoryInfo = new System.IO.DirectoryInfo(projectPath);

                var restoreQgisProjectDirectoryInfo = new System.IO.DirectoryInfo(projectPath);

                currentQgisProjectDirectoryInfo.MoveTo(System.IO.Path.Combine(qgisProjectsRootPath, $"{key}_backup"));

                try
                {
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        model.MapArchive.CopyTo(memoryStream);

                        using (var uploadZipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read, false, System.Text.CodePagesEncodingProvider.Instance.GetEncoding(System.Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)))
                        {
                            uploadZipArchive.ExtractToDirectory(qgisProjectsRootPath, true);
                        }

                        if (!restoreQgisProjectDirectoryInfo.Exists)
                        {
                            throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", "Estructura del empaqutado incorrecta!!!");
                        }

                        if (!System.IO.File.Exists(System.IO.Path.Combine(restoreQgisProjectDirectoryInfo.FullName, string.Format("{0}.qgz", key))))
                        {
                            throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", "Archivo de proyecto no encontrado!!!");
                        }

                        using (var httpClient = new System.Net.Http.HttpClient())
                        {
                            var uri = string.Format("{0}?MAP={1}/{2}.qgz&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetCapabilities", configuration["QGIS:ServerBaseUri"], restoreQgisProjectDirectoryInfo.FullName.Replace("\\", "/"), key);

                            var httpResponseMessage = await httpClient.GetAsync(uri);

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                                var projDescriptorXmlDocument = new System.Xml.XmlDocument();

                                projDescriptorXmlDocument.Load(contentStream);

                                var globalLayerNode = projDescriptorXmlDocument.ChildNodes[1].ChildNodes[1].ChildNodes[3];

                                var globalLayerNodeChildNodesCount = globalLayerNode.ChildNodes.Count;

                                foreach (var layer in entity.Layers)
                                {
                                    layer.Enabled = false;
                                }

                                for (int i = 0; i < globalLayerNodeChildNodesCount; i++)
                                {
                                    var globalLayerNodeChild = globalLayerNode.ChildNodes[i];

                                    switch (globalLayerNodeChild.Name)
                                    {
                                        case "Title":
                                            entity.Name = globalLayerNodeChild.InnerText;

                                            break;

                                        case "CRS":
                                            var crsValue = globalLayerNodeChild.InnerText;

                                            if (crsValue.StartsWith("CRS:"))
                                            {
                                                entity.CRS = crsValue;
                                            }
                                            else
                                            {
                                                entity.EPSG = crsValue;
                                            }

                                            break;

                                        case "EX_GeographicBoundingBox":
                                            entity.WestBoundLongitude = decimal.Parse(globalLayerNodeChild.ChildNodes[0].InnerText.Replace(".", ","));
                                            entity.EastBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[1].InnerText.Replace(".", ","));
                                            entity.SouthBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[2].InnerText.Replace(".", ","));
                                            entity.NorthBoundLatitude = decimal.Parse(globalLayerNodeChild.ChildNodes[3].InnerText.Replace(".", ","));

                                            break;

                                        case "Layer":
                                            var childs = FindSubLayers(globalLayerNodeChild);

                                            if (childs.Count > 0)
                                            {
                                                foreach (var item in childs)
                                                {
                                                    var layer = entity.Layers.SingleOrDefault(_entity => _entity.Name == item.Value.Name);

                                                    if (layer != null)
                                                    {
                                                        layer.Title = item.Value.Title;
                                                        layer.Enabled = true;
                                                        layer.CRS = item.Value.CRS;
                                                        layer.EPSG = item.Value.EPSG;

                                                        continue;
                                                    }

                                                    entity.Layers.Add(item.Value);
                                                }
                                            }
                                            else
                                            {
                                                var layerName = globalLayerNodeChild.ChildNodes[0].InnerText;

                                                var layer = entity.Layers.SingleOrDefault(_entity => _entity.Name == layerName);

                                                if (layer != null)
                                                {
                                                    layer.Title = globalLayerNodeChild.ChildNodes[1].InnerText;
                                                    layer.Enabled = true;
                                                    layer.CRS = globalLayerNodeChild.ChildNodes[2].InnerText;
                                                    layer.EPSG = globalLayerNodeChild.ChildNodes[3].InnerText;
                                                }
                                                else
                                                {
                                                    layer = new Layer
                                                    {
                                                        Name = globalLayerNodeChild.ChildNodes[0].InnerText,
                                                        Title = globalLayerNodeChild.ChildNodes[1].InnerText,
                                                        Enabled = true,
                                                        Priority = 1,
                                                        PresentationStrategy = Camaguey.Persistence.Enumerations.LayerPresentationStrategy.WMS,
                                                        CRS = globalLayerNodeChild.ChildNodes[2].InnerText,
                                                        EPSG = globalLayerNodeChild.ChildNodes[3].InnerText,
                                                    };

                                                    entity.Layers.Add(layer);
                                                }
                                            }

                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(entity.Name)) entity.Name = entity.Id;

                                commandRepository.Update(entity, User.Identity.Name, true);
                            }
                            else
                            {
                                throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", string.Format("Archivo de proyecto no interpretable por el servidor de QGIS. Código del error: {0}!!!", httpResponseMessage.StatusCode));
                            }
                        }
                    }

                    try
                    {
                        if (currentQgisProjectDirectoryInfo.Exists)
                        {
                            currentQgisProjectDirectoryInfo.Delete(true);
                        }
                    }
                    catch { }

                    SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                    return RedirectToAction("Edit", new { key });
                }
                catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                {
                    string message = $"Problemas cargando archivo de proyecto QGIS : {e.ErrorMessage}";

                    logger.LogError(e, message);

                    SetDefaultNotification(NotificationType.Danger, message);

                    ModelState.AddModelError(e.ModelKey, e.ErrorMessage);

                    if (restoreQgisProjectDirectoryInfo.Exists)
                    {
                        restoreQgisProjectDirectoryInfo.Delete(true);
                    }

                    currentQgisProjectDirectoryInfo.MoveTo(projectPath);
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, "Excepción no esperada cargando archivo de proyect QGIS!!!");

                    SetDefaultNotification(NotificationType.Danger, "Error no esperado cargando archivo de proyect QGIS!!!");

                    if (restoreQgisProjectDirectoryInfo.Exists)
                    {
                        restoreQgisProjectDirectoryInfo.Delete(true);
                    }

                    currentQgisProjectDirectoryInfo.MoveTo(projectPath);
                }
            }
            else
            {
                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());
            }

            return RedirectToAction("Edit", new { key });
        }

        protected override void PreEdit(Map entity, MapInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (model.MetadataFile != null)
            {
                using (var streamReader = new System.IO.StreamReader(model.MetadataFile.OpenReadStream(), Encoding.UTF8))
                {
                    entity.MetadataEntries.Clear();

                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var values = line.Split(";");

                        entity.MetadataEntries.Add(new MapMetadataEntry
                        {
                            MapId = entity.Id,
                            Map = entity,
                            Name = values[0],
                            Value = values[1]
                        });
                    }
                };
            }

            if (model.ImageFile == null)
            {
                if (model.ResetImage && entity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Image);

                    commandImageRepository.Delete(entity.Image, User.Identity.Name);
                }

                return;
            }

            var deprecatedImage = entity.Image;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = commandImageRepository.Add(new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Map " + entity.Id }, User.Identity.Name);

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.ImageId = image.Id;
                entity.Image = image;
            }

            if (deprecatedImage != null)
            {
                imageFileManager.DeleteImageFile(deprecatedImage);

                commandImageRepository.Delete(deprecatedImage, User.Identity.Name);
            }
        }

        protected override void PostDelete(Map entity)
        {
            base.PostDelete(entity);

            if (entity.Image == null) return;

            commandImageRepository.Delete(entity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Image);
        }

        #region Layers

        [HttpGet]
        [Route("edit/{key}/layers")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditLayers(string key)
        {
            var entity = queryRepository.Get(key);

            var model = new Models.View.Backend.Management.MapLayersManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Layers = entity.Layers.Select(item =>
                {
                    var result = new LayerDisplayViewModel();

                    result.Import(item);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/layers/edit")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditLayer(string key, System.Guid layerId, bool enabled, int priority, Camaguey.Persistence.Enumerations.LayerPresentationStrategy presentationStrategy, string stylePresentantionJSBlock, string pointToPresentationJSBlock, string interactivePresentantionJSBlock)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = commandRepository.Get(key);

                    try
                    {
                        var _entity = entity.Layers.First(b => b.Id == layerId);

                        _entity.Enabled = enabled;
                        _entity.Priority = priority;
                        _entity.PresentationStrategy = presentationStrategy;
                        _entity.StylePresentantionJSBlock = stylePresentantionJSBlock;
                        _entity.PointToPresentationJSBlock = pointToPresentationJSBlock;
                        _entity.InteractivePresentantionJSBlock = interactivePresentantionJSBlock;

                        commandLayerRepository.Update(_entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("editlayers", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }

                    SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                    return RedirectToAction("editlayers", new { key = entity.Id });
                }
                catch (System.Exception e)
                {
                    logger.LogError(e.InnerException, e.Message);
                }
            }

            SetDefaultNotification(notificationType: NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        #endregion

        #region Authors

        [HttpGet]
        [Route("edit/{key}/authors")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditAuthors(string key)
        {
            var entity = queryRepository.Get(key);

            var releatedMaps = entity.AuthorBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new Models.View.Backend.Management.MapAuthorsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Authors = entity.AuthorBridges.Select(bridge =>
                {
                    var result = new AuthorDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableAuthorSelectListItems = queryAuthorRepository.GetAll().ToArray().Except(releatedMaps).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = entity.Id.ToString(),
                    Text = entity.Identity.LastName == null ? entity.Identity.Name : string.Format("{0} {1}", entity.Identity.Name, entity.Identity.LastName)
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/authors/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddAuthor(string key, System.Guid authorId)
        {
            try
            {
                var entity = commandRepository.Get(key);

                if (authorId != default)
                {
                    try
                    {
                        entity.AuthorBridges.Add(new MapAuthorBridge
                        {
                            ProwId = entity.Id,
                            SternId = authorId
                        });

                        commandRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editauthors", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editauthors", new { key = entity.Id });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/authors/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeleteAuthor(string key, System.Guid authorId)
        {
            try
            {
                var entity = commandRepository.Get(key);

                var bridge = entity.AuthorBridges.First(b => b.SternId == authorId);

                entity.AuthorBridges.Remove(bridge);

                commandRepository.Update(entity, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editauthors", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editauthors", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion

        #region KeyWords

        [HttpGet]
        [Route("edit/{key}/keywords")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditKeyWords(string key)
        {
            var entity = queryRepository.Get(key);

            var releatedKeyWords = entity.KeyWordBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new Models.View.Backend.Management.MapKeyWordsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                KeyWords = entity.KeyWordBridges.Select(bridge =>
                {
                    var result = new KeyWordDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableKeyWordSelectListItems = queryKeyWordRepository.GetAll().ToArray().Except(releatedKeyWords).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = entity.Id.ToString(),
                    Text = entity.Name
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/keywords/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddKeyWord(string key, System.Guid keyWordId)
        {
            try
            {
                var entity = commandRepository.Get(key);

                if (keyWordId != default)
                {
                    try
                    {
                        entity.KeyWordBridges.Add(new MapKeyWordBridge
                        {
                            ProwId = entity.Id,
                            SternId = keyWordId
                        });

                        commandRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editkeywords", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editkeywords", new { key = entity.Id });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/keywords/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeleteKeyWord(string key, System.Guid keyWordId)
        {
            try
            {
                var entity = commandRepository.Get(key);

                var bridge = entity.KeyWordBridges.First(b => b.SternId == keyWordId);

                entity.KeyWordBridges.Remove(bridge);

                commandRepository.Update(entity, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editkeywords", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editkeywords", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion

        #region Graphs

        [HttpGet]
        [Route("edit/{key}/graphs")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditGraphs(string key)
        {
            var entity = queryRepository.Get(key);

            var model = new Models.View.Backend.Management.MapGraphsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Images = entity.Images.Where(_entity => _entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Chart).Select(_entity =>
                {
                    var result = new ImageDisplayViewModel();

                    result.Import(_entity.Image);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/graphs/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddGraph(string key, ImageInputViewModel model)
        {
            try
            {
                var entity = queryRepository.Get(key);

                ModelState.ClearValidationState("File");

                model.File = Request.Form.Files[0];

                if (Request.Form.Files.Count > 1)
                {
                    model.MatadataFile = Request.Form.Files[1];
                }

                TryValidateModel(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            model.File.CopyTo(memoryStream);

                            var image = new Image { ContentType = model.File.ContentType, Length = model.File.Length, About = model.Description };

                            commandImageRepository.Add(image, User.Identity.Name, false);

                            imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.File.ContentType);

                            var _entity = new MapImage
                            {
                                MapId = entity.Id,
                                ImageId = image.Id,
                                ImageKind = Camaguey.Persistence.Enumerations.MapImageKind.Chart
                            };

                            if (model.MatadataFile != null)
                            {
                                using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        var line = streamReader.ReadLine();
                                        var values = line.Split(";");

                                        _entity.MetadataEntries.Add(new MapImageMetadataEntry
                                        {
                                            MapImageId = _entity.Id,
                                            MapImage = _entity,
                                            Name = values[0],
                                            Value = values[1]
                                        });
                                    }
                                };
                            }

                            commandMapImageRepository.Add(_entity, User.Identity.Name);
                        }

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("EditGraphs", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("EditGraphs", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/graphs/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeleteGraph(string key, System.Guid imageId)
        {
            try
            {
                var entity = queryRepository.Get(key);

                var _entity = entity.Images.First(__entity => __entity.ImageId == imageId && __entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Chart);

                var deprecatedImage = _entity.Image;

                commandImageRepository.Delete(_entity.Image, User.Identity.Name);

                try
                {
                    imageFileManager.DeleteImageFile(deprecatedImage);
                }
                catch { }

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("EditGraphs", new { key });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("EditGraphs", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/graphs/edit")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditGraph(string key, ImageInputViewModel model)
        {
            try
            {
                ModelState.ClearValidationState("File");
                ModelState.MarkFieldValid("File");

                if (Request.Form.Files.Count > 0)
                {
                    model.MatadataFile = Request.Form.Files[0];

                    TryValidateModel(model);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var entity = queryMapImageRepository.FindOneBy(_entity => _entity.ImageId == model.Id && _entity.MapId == key && _entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Chart);

                        entity = commandMapImageRepository.Get(entity.Id);

                        entity.Image.About = model.Description;

                        if (model.MatadataFile != null)
                        {
                            entity.MetadataEntries.Clear();

                            using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    var line = streamReader.ReadLine();

                                    var values = line.Split(";");

                                    entity.MetadataEntries.Add(new MapImageMetadataEntry
                                    {
                                        MapImageId = entity.Id,
                                        MapImage = entity,
                                        Name = values[0],
                                        Value = values[1]
                                    });
                                }
                            };
                        }

                        commandMapImageRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("EditGraphs", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("EditGraphs", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion

        #region Pictures

        [HttpGet]
        [Route("edit/{key}/pictures")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditPictures(string key)
        {
            var entity = queryRepository.Get(key);

            var model = new Models.View.Backend.Management.MapPicturesManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Images = entity.Images.Where(_entity => _entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Picture).Select(_entity =>
                {
                    var result = new ImageDisplayViewModel();

                    result.Import(_entity.Image);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/pictures/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddPicture(string key, ImageInputViewModel model)
        {
            try
            {
                var entity = queryRepository.Get(key);

                ModelState.ClearValidationState("File");

                model.File = Request.Form.Files[0];

                if (Request.Form.Files.Count > 1)
                {
                    model.MatadataFile = Request.Form.Files[1];
                }

                TryValidateModel(model);

                if(ModelState.IsValid)
                {
                    try
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            model.File.CopyTo(memoryStream);

                            var image = new Image { ContentType = model.File.ContentType, Length = model.File.Length, About = model.Description };

                            commandImageRepository.Add(image, User.Identity.Name, false);

                            imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.File.ContentType);

                            var _entity = new MapImage
                            {
                                MapId = entity.Id,
                                ImageId = image.Id,
                                ImageKind = Camaguey.Persistence.Enumerations.MapImageKind.Picture
                            };

                            if (model.MatadataFile != null)
                            {
                                using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        var line = streamReader.ReadLine();
                                        var values = line.Split(";");

                                        _entity.MetadataEntries.Add(new MapImageMetadataEntry
                                        {
                                            MapImageId = _entity.Id,
                                            MapImage = _entity,
                                            Name = values[0],
                                            Value = values[1]
                                        });
                                    }
                                };
                            }

                            commandMapImageRepository.Add(_entity, User.Identity.Name);
                        }

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("EditPictures", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("EditPictures", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/pictures/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeletePicture(string key, System.Guid imageId)
        {
            try
            {
                var entity = queryRepository.Get(key);

                var _entity = entity.Images.First(__entity => __entity.ImageId == imageId && __entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Picture);

                var deprecatedImage = _entity.Image;

                commandImageRepository.Delete(_entity.Image, User.Identity.Name);

                try
                {
                    imageFileManager.DeleteImageFile(deprecatedImage);
                }
                catch { }

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("EditPictures", new { key });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("EditPictures", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/pictures/edit")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditPicture(string key, ImageInputViewModel model)
        {
            try
            {
                ModelState.ClearValidationState("File");
                ModelState.MarkFieldValid("File");

                if (Request.Form.Files.Count > 0)
                {
                    model.MatadataFile = Request.Form.Files[0];

                    TryValidateModel(model);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var entity = queryMapImageRepository.FindOneBy(_entity => _entity.ImageId == model.Id && _entity.MapId == key && _entity.ImageKind == Camaguey.Persistence.Enumerations.MapImageKind.Picture);

                        entity = commandMapImageRepository.Get(entity.Id);

                        entity.Image.About = model.Description;

                        if (model.MatadataFile != null)
                        {
                            entity.MetadataEntries.Clear();

                            using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    var line = streamReader.ReadLine();

                                    var values = line.Split(";");

                                    entity.MetadataEntries.Add(new MapImageMetadataEntry
                                    {
                                        MapImageId = entity.Id,
                                        MapImage = entity,
                                        Name = values[0],
                                        Value = values[1]
                                    });
                                }
                            };
                        }

                        commandMapImageRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("EditPictures", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("EditPictures", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion

        #region Videos

        [HttpGet]
        [Route("edit/{key}/videos")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditVideos(string key)
        {
            var entity = queryRepository.Get(key);

            var model = new Models.View.Backend.Management.MapVideosManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Videos = entity.Videos.Select(_entity =>
                {
                    var result = new VideoDisplayViewModel();

                    result.Import(_entity.Video);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/videos/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddVideo(string key, VideoInputViewModel model)
        {
            try
            {
                var entity = queryRepository.Get(key);

                ModelState.ClearValidationState("File");

                model.File = Request.Form.Files[0];

                if (Request.Form.Files.Count > 1)
                {
                    model.MatadataFile = Request.Form.Files[1];
                }

                TryValidateModel(model);

                if (ModelState.IsValid)
                {
                    var video = new Video
                    {
                        Extension = model.File.ContentType.Split("/")[1],
                        Length = model.File.Length,
                        About = model.Description,
                        Format = model.File.ContentType,
                        FrameSize = "unknown",
                        AudioChannelOutput = "unknown",
                        AudioFormat = "unknown",
                        ColorModel = "unknown",
                        Duration = System.TimeSpan.Zero,
                        Fps = 60
                    };

                    try
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            model.File.CopyTo(memoryStream);

                            commandVideoRepository.Add(video, User.Identity.Name, false);

                            videoFileManager.SaveVideoFileContent(video.Id, memoryStream.ToArray(), video.Extension);

                            var _entity = new MapVideo
                            {
                                MapId = entity.Id,
                                VideoId = video.Id
                            };

                            if (model.MatadataFile != null)
                            {
                                using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        var line = streamReader.ReadLine();
                                        var values = line.Split(";");

                                        _entity.MetadataEntries.Add(new MapVideoMetadataEntry
                                        {
                                            MapVideoId = _entity.Id,
                                            MapVideo = _entity,
                                            Name = values[0],
                                            Value = values[1]
                                        });
                                    }
                                };
                            }

                            commandMapVideoRepository.Add(_entity, User.Identity.Name);
                        }

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editvideos", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);

                        try
                        {
                            videoFileManager.DeleteVideoFile(video.Id, video.Extension);
                        }
                        catch { }
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editvideos", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/videos/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeleteVideo(string key, System.Guid videoId)
        {
            try
            {
                var entity = queryRepository.Get(key);

                var _entity = entity.Videos.First(b => b.VideoId == videoId);

                var deprecatedVideo = _entity.Video;

                commandVideoRepository.Delete(_entity.Video, User.Identity.Name);

                try
                {
                    videoFileManager.DeleteVideoFile(deprecatedVideo.Id, deprecatedVideo.Extension);
                }
                catch { }

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editvideos", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editvideos", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/videos/edit")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditVideo(string key, VideoInputViewModel model)
        {
            try
            {
                ModelState.ClearValidationState("File");
                ModelState.MarkFieldValid("File");

                if (Request.Form.Files.Count > 0)
                {
                    model.MatadataFile = Request.Form.Files[0];

                    TryValidateModel(model);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var entity = queryMapVideoRepository.FindOneBy(_entity => _entity.VideoId == model.Id && _entity.MapId == key);

                        entity = commandMapVideoRepository.Get(entity.Id);

                        entity.Video.About = model.Description;

                        if (model.MatadataFile != null)
                        {
                            entity.MetadataEntries.Clear();

                            using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    var line = streamReader.ReadLine();

                                    var values = line.Split(";");

                                    entity.MetadataEntries.Add(new MapVideoMetadataEntry
                                    {
                                        MapVideoId = entity.Id,
                                        MapVideo = entity,
                                        Name = values[0],
                                        Value = values[1]
                                    });
                                }
                            };
                        }

                        commandMapVideoRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("editvideos", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editvideos", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion

        #region Documents

        [HttpGet]
        [Route("edit/{key}/documents")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditDocuments(string key)
        {
            var entity = queryRepository.Get(key);

            var model = new Models.View.Backend.Management.MapDocumentsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Documents = entity.Documents.Select(item =>
                {
                    var result = new DocumentDisplayViewModel();

                    result.Import(item.Document);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/documents/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddDocument(string key, DocumentInputViewModel model)
        {
            try
            {
                var entity = queryRepository.Get(key);

                ModelState.ClearValidationState("File");

                model.File = Request.Form.Files[0];

                if (Request.Form.Files.Count > 1)
                {
                    model.MatadataFile = Request.Form.Files[1];
                }

                TryValidateModel(model);

                if (ModelState.IsValid)
                {
                    try
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            model.File.CopyTo(memoryStream);

                            var document = new Document
                            {
                                Name = model.Name,
                                Description = model.Description,
                                ContentType = model.File.ContentType,
                                Length = model.File.Length
                            };

                            commandDocumentRepository.Add(document, User.Identity.Name, false);

                            documentFileManager.SaveDocumentFileContent(document.Id, memoryStream.ToArray(), document.ContentType);

                            var _entity = new MapDocument
                            {
                                MapId = entity.Id,
                                DocumentId = document.Id
                            };

                            if (model.MatadataFile != null)
                            {
                                using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                                {
                                    while (!streamReader.EndOfStream)
                                    {
                                        var line = streamReader.ReadLine();

                                        var values = line.Split(";");
                                        
                                        _entity.MetadataEntries.Add(new MapDocumentMetadataEntry
                                        {
                                            MapDocumentId = _entity.Id,
                                            MapDocument = _entity,
                                            Name = values[0],
                                            Value = values[1]
                                        });
                                    }
                                };
                            }

                            commandMapDocumentRepository.Add(_entity, User.Identity.Name);
                        }

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editdocuments", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editdocuments", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/documents/remove")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult RemoveDocument(string key, System.Guid documentId)
        {
            try
            {
                var entity = queryRepository.Get(key);

                var _entity = entity.Documents.First(b => b.DocumentId == documentId);

                var deprecatedDocument = _entity.Document;

                commandDocumentRepository.Delete(_entity.Document, User.Identity.Name);

                try
                {
                    documentFileManager.DeleteDocumentFile(deprecatedDocument);
                }
                catch { }

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editdocuments", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editdocuments", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/documents/edit")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditDocument(string key, DocumentInputViewModel model)
        {
            try
            {
                ModelState.ClearValidationState("File");
                ModelState.MarkFieldValid("File");

                if (Request.Form.Files.Count > 0)
                {
                    model.MatadataFile = Request.Form.Files[0];

                    TryValidateModel(model);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var entity = queryMapDocumentRepository.FindOneBy(_entity => _entity.DocumentId == model.Id && _entity.MapId == key);

                        entity = commandMapDocumentRepository.Get(entity.Id);

                        entity.Document.Name = model.Name;
                        entity.Document.Description = model.Description;

                        if (model.MatadataFile != null)
                        {
                            entity.MetadataEntries.Clear();

                            using (var streamReader = new System.IO.StreamReader(model.MatadataFile.OpenReadStream(), Encoding.UTF8))
                            {
                                while (!streamReader.EndOfStream)
                                {
                                    var line = streamReader.ReadLine();

                                    var values = line.Split(";");

                                    entity.MetadataEntries.Add(new MapDocumentMetadataEntry
                                    {
                                        MapDocumentId = entity.Id,
                                        MapDocument = entity,
                                        Name = values[0],
                                        Value = values[1]
                                    });
                                }
                            };
                        }

                        commandMapDocumentRepository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("editdocuments", new { key });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editdocuments", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(notificationType: NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        #endregion

        private static List<KeyValuePair<System.Xml.XmlNode, Layer>> FindSubLayers(System.Xml.XmlNode layerGroupNode)
        {
            var result = new List<KeyValuePair<System.Xml.XmlNode, Layer>>();

            var childNodesCount = layerGroupNode.ChildNodes.Count;

            for (int i = 0; i < childNodesCount; i++)
            {
                var nodeChild = layerGroupNode.ChildNodes[i];

                if (nodeChild.Name != "Layer") continue;

                var layerChilds = FindSubLayers(nodeChild);

                if (layerChilds.Count > 0)
                {
                    result.AddRange(layerChilds);
                }
                else
                {
                    var layer = new Layer
                    {
                        Name = nodeChild.ChildNodes[0].InnerText,
                        Title = nodeChild.ChildNodes[1].InnerText,
                        Enabled = true,
                        Priority = 1,
                        PresentationStrategy = Camaguey.Persistence.Enumerations.LayerPresentationStrategy.WMS,
                        CRS = nodeChild.ChildNodes[2].InnerText,
                        EPSG = nodeChild.ChildNodes[3].InnerText,
                    };

                    result.Add(new KeyValuePair<System.Xml.XmlNode, Layer>(nodeChild, layer));
                }
            }

            return result;
        }
    }
}

