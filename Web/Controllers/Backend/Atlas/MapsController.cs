using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Models.View;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class MapsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<Map, string, MapInputViewModel, MapDisplayViewModel>
    {
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Document, System.Guid> documentRepository;
        private readonly IEntityRepository<Video, System.Guid> videoRepository;
        private readonly IEntityRepository<KeyWord, System.Guid> keyWordRepository;
        private readonly IEntityRepository<Author, System.Guid> authorRepository;
        private readonly IEntityRepository<Layer, System.Guid> layerRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IDocumentFileManagerFacade documentFileManager;
        private readonly IVideoFileManagerFacade videoFileManager;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public MapsController(Microsoft.Extensions.Configuration.IConfiguration configuration, IVideoFileManagerFacade videoFileManager, IDocumentFileManagerFacade documentFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Layer, System.Guid> layerRepository, IEntityRepository<Author, System.Guid> authorRepository, IEntityRepository<KeyWord, System.Guid> keyWordRepository, IEntityRepository<Video, System.Guid> videoRepository, IEntityRepository<Document, System.Guid> documentRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Map, string> repository, IStringLocalizer<MapsController> localizer, ILogger<MapsController> logger) : base(repository, localizer, logger)
        {
            this.imageRepository = imageRepository;
            this.documentRepository = documentRepository;
            this.videoRepository = videoRepository;
            this.keyWordRepository = keyWordRepository;
            this.authorRepository = authorRepository;
            this.layerRepository = layerRepository;
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

        protected override System.Func<Map, object> GetOrderSelector()
        {
            return entity => entity.Name;
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
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    model.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(model);

                    updateModelTask.Wait();
                }
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

                var qgisProjectDirectoryInfo = new System.IO.DirectoryInfo(System.IO.Path.Combine(qgisProjectsRootPath, mapId.ToUpperInvariant()));

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

                        if (!System.IO.File.Exists(System.IO.Path.Combine(qgisProjectDirectoryInfo.FullName, string.Format("{0}.qgz", mapId.ToUpperInvariant()))))
                        {
                            throw new Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException("MapArchive", "Archivo de proyecto no encontrado!!!");
                        }

                        using (var httpClient = new System.Net.Http.HttpClient())
                        {
                            var uri = string.Format("{0}?MAP={1}/{2}.qgz&SERVICE=WMS&VERSION=1.3.0&REQUEST=GetCapabilities", configuration["QGIS:ServerBaseUri"], qgisProjectDirectoryInfo.FullName.Replace("\\", "/"), mapId.ToUpperInvariant());

                            var httpResponseMessage = await httpClient.GetAsync(uri);

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                using (var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync())
                                {
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
                                                        CRS = globalLayerNodeChild.ChildNodes[2].InnerText,
                                                        EPSG = globalLayerNodeChild.ChildNodes[3].InnerText,
                                                    };

                                                    map.Layers.Add(layer);
                                                }

                                                break;
                                        }                                        
                                    }

                                    if (string.IsNullOrEmpty(map.Name)) map.Name = map.Id;

                                    Repository.Add(map, User.Identity.Name, true);
                                }
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
                    logger.LogWarning(e, "Problemas cargando archivo de proyecto QGIS.");

                    ModelState.AddModelError(e.ModelKey, e.ErrorMessage);

                    if (qgisProjectDirectoryInfo.Exists)
                    {
                        qgisProjectDirectoryInfo.Delete(true);
                    }
                }
                catch (System.Exception e)
                {
                    logger.LogError(e, "Excepción no esperada cargando archivo de proyect QGIS!!!");

                    if (qgisProjectDirectoryInfo.Exists)
                    {
                        qgisProjectDirectoryInfo.Delete(true);
                    }
                }
            }

            SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

            return RedirectToAction("Index");
        }

        protected override void PreEdit(Map entity, MapInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (model.ImageFile == null)
            {
                if (model.ResetImage && entity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Image);

                    imageRepository.Delete(entity.Image, User.Identity.Name);
                }

                return;
            }

            var deprecatedImage = entity.Image;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = imageRepository.Add(new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Map " + entity.Id }, User.Identity.Name);

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.ImageId = image.Id;
                entity.Image = image;
            }

            if (deprecatedImage != null)
            {
                imageFileManager.DeleteImageFile(deprecatedImage);

                imageRepository.Delete(deprecatedImage, User.Identity.Name);
            }
        }

        protected override void PostDelete(Map entity)
        {
            base.PostDelete(entity);

            if (entity.Image == null) return;

            imageRepository.Delete(entity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Image);
        }

        #region Layers

        [HttpGet]
        [Route("edit/{key}/layers")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditLayers(string key)
        {
            var entity = Repository.Get(key);

            var model = new MapLayersManagementViewModel
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
        public IActionResult EditLayer(string key, System.Guid layerId, bool enabled, int priority, string stylePresentantionJSBlock, string pointToPresentationJSBlock, string interactivePresentantionJSBlock)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = Repository.Get(key);

                    try
                    {
                        var _entity = entity.Layers.First(b => b.Id == layerId);

                        _entity.Enabled = enabled;
                        _entity.Priority = priority;
                        _entity.StylePresentantionJSBlock = stylePresentantionJSBlock;
                        _entity.PointToPresentationJSBlock = pointToPresentationJSBlock;
                        _entity.InteractivePresentantionJSBlock = interactivePresentantionJSBlock;

                        layerRepository.Update(_entity, User.Identity.Name);

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
            var entity = Repository.Get(key);

            var releatedMaps = entity.AuthorBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new MapAuthorsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Authors = entity.AuthorBridges.Select(bridge =>
                {
                    var result = new AuthorDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableAuthorSelectListItems = authorRepository.GetAll().ToArray().Except(releatedMaps).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
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
                var entity = Repository.Get(key);

                if (authorId != default)
                {
                    try
                    {
                        entity.AuthorBridges.Add(new MapAuthorBridge
                        {
                            ProwId = entity.Id,
                            SternId = authorId
                        });

                        Repository.Update(entity, User.Identity.Name);

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
                var entity = Repository.Get(key);

                var bridge = entity.AuthorBridges.First(b => b.SternId == authorId);

                entity.AuthorBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

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
            var entity = Repository.Get(key);

            var releatedKeyWords = entity.KeyWordBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new MapKeyWordsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                KeyWords = entity.KeyWordBridges.Select(bridge =>
                {
                    var result = new KeyWordDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableKeyWordSelectListItems = keyWordRepository.GetAll().ToArray().Except(releatedKeyWords).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
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
                var entity = Repository.Get(key);

                if (keyWordId != default)
                {
                    try
                    {
                        entity.KeyWordBridges.Add(new MapKeyWordBridge
                        {
                            ProwId = entity.Id,
                            SternId = keyWordId
                        });

                        Repository.Update(entity, User.Identity.Name);

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
                var entity = Repository.Get(key);

                var bridge = entity.KeyWordBridges.First(b => b.SternId == keyWordId);

                entity.KeyWordBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

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

        #region Images

        [HttpGet]
        [Route("edit/{key}/images")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditImages(string key)
        {
            var entity = Repository.Get(key);

            var model = new MapImagesManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Images = entity.ImageBridges.Select(bridge =>
                {
                    var result = new ImageDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray()
            };

            model.Map.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/images/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult AddImage(string key)
        {
            try
            {
                var imageFile = Request.Form.Files[0];

                var entity = Repository.Get(key);

                // TODO Validate imageFile

                if (ModelState.IsValid)
                {
                    try
                    {
                        using (var memoryStream = new System.IO.MemoryStream())
                        {
                            imageFile.CopyTo(memoryStream);

                            var image = new Image { ContentType = imageFile.ContentType, Length = imageFile.Length, About = "Map Compl. " + entity.Id };

                            imageRepository.Add(image, User.Identity.Name, false);

                            imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), imageFile.ContentType);

                            entity.ImageBridges.Add(new MapImageBridge
                            {
                                ProwId = entity.Id,
                                SternId = image.Id
                            });
                        }

                        Repository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editimages", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editimages", new { key = entity.Id });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/images/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult DeleteImage(string key, System.Guid imageId)
        {
            try
            {
                var entity = Repository.Get(key);

                var bridge = entity.ImageBridges.First(b => b.SternId == imageId);

                entity.ImageBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

                imageRepository.Delete(bridge.Stern, User.Identity.Name);

                imageFileManager.DeleteImageFile(bridge.Stern);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editimages", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editimages", new { key });
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
            var entity = Repository.Get(key);

            var model = new MapVideosManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Videos = entity.VideoBridges.Select(bridge =>
                {
                    var result = new VideoDisplayViewModel();

                    result.Import(bridge.Stern);

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
        public IActionResult AddVideo(string key, string about)
        {
            try
            {
                var videoFile = Request.Form.Files[0];

                var entity = Repository.Get(key);

                // TODO Validate videoFile & ...

                if (ModelState.IsValid)
                {
                    var video = new Video
                    {
                        Extension = videoFile.ContentType.Split("/")[1],
                        Length = videoFile.Length,
                        About = about,
                        Format = videoFile.ContentType,
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
                            videoFile.CopyTo(memoryStream);

                            videoRepository.Add(video, User.Identity.Name, false);

                            videoFileManager.SaveVideoFileContent(video.Id, memoryStream.ToArray(), video.Extension);

                            entity.VideoBridges.Add(new MapVideoBridge
                            {
                                ProwId = entity.Id,
                                SternId = video.Id
                            });
                        }

                        Repository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editvideos", new { key = entity.Id });
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

                return RedirectToAction("editvideos", new { key = entity.Id });
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
                var entity = Repository.Get(key);

                var bridge = entity.VideoBridges.First(b => b.SternId == videoId);

                entity.VideoBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

                videoRepository.Delete(bridge.Stern, User.Identity.Name);

                videoFileManager.DeleteVideoFile(bridge.SternId, bridge.Stern.Extension);

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
        public IActionResult EditVideo(string key, System.Guid videoId, string about)
        {
            try
            {
                var entity = Repository.Get(key);

                var bridge = entity.VideoBridges.First(b => b.SternId == videoId);

                bridge.Stern.About = about;

                Repository.Update(entity, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

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
            var entity = Repository.Get(key);

            var model = new MapDocumentsManagementViewModel
            {
                Map = new MapDisplayViewModel(),
                Documents = entity.DocumentBridges.Select(item =>
                {
                    var result = new DocumentDisplayViewModel();

                    result.Import(item.Stern);

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
                var entity = Repository.Get(key);

                ModelState.ClearValidationState("File");

                model.File = Request.Form.Files[0];

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

                            documentRepository.Add(document, User.Identity.Name, false);

                            entity.DocumentBridges.Add(new MapDocumentBridge
                            {
                                ProwId = entity.Id,
                                SternId = document.Id
                            });

                            documentFileManager.SaveDocumentFileContent(document.Id, memoryStream.ToArray(), document.ContentType);

                            Repository.Update(entity, User.Identity.Name);
                        }

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editdocuments", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editdocuments", new { key = entity.Id });
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
                var entity = Repository.Get(key);

                var bridge = entity.DocumentBridges.First(b => b.SternId == documentId);

                entity.DocumentBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

                try
                {
                    documentFileManager.DeleteDocumentFile(bridge.Stern);
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
        public IActionResult EditDocument(string key, DocumentInputViewModel modelInput)
        {
            ModelState.ClearValidationState("File");
            ModelState.MarkFieldValid("File");

            if (ModelState.IsValid)
            {
                try
                {
                    var entity = Repository.Get(key);

                    try
                    {
                        var bridge = entity.DocumentBridges.First(b => b.SternId == modelInput.Id);

                        bridge.Stern.Name = modelInput.Name;
                        bridge.Stern.Description = modelInput.Description;

                        Repository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetEditionSuccessMessage());

                        return RedirectToAction("editdocuments", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }

                    SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                    return RedirectToAction("editdocuments", new { key = entity.Id });
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

        private List<KeyValuePair<System.Xml.XmlNode, Layer>> FindSubLayers(System.Xml.XmlNode layerGroupNode)
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

