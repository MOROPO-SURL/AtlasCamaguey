using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Models.View;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class MapsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Map, System.Guid, MapInputViewModel, MapDisplayViewModel>
    {
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Document, System.Guid> documentRepository;
        private readonly IEntityRepository<Video, System.Guid> videoRepository;
        private readonly IEntityRepository<KeyWord, System.Guid> keyWordRepository;
        private readonly IEntityRepository<Author, System.Guid> authorRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IDocumentFileManagerFacade documentFileManager;
        private readonly IVideoFileManagerFacade videoFileManager;

        public MapsController(IVideoFileManagerFacade videoFileManager, IDocumentFileManagerFacade documentFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<Author, System.Guid> authorRepository, IEntityRepository<KeyWord, System.Guid> keyWordRepository, IEntityRepository<Video, System.Guid> videoRepository, IEntityRepository<Document, System.Guid> documentRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Map, System.Guid> repository, IStringLocalizer<MapsController> localizer, ILogger<MapsController> logger) : base(repository, localizer, logger)
        {
            this.imageRepository = imageRepository;
            this.documentRepository = documentRepository;
            this.videoRepository = videoRepository;
            this.keyWordRepository = keyWordRepository;
            this.authorRepository = authorRepository;
            this.imageFileManager = imageFileManager;
            this.documentFileManager = documentFileManager;
            this.videoFileManager = videoFileManager;
        }

        [RequirePermissions(Constants.Permission.MapViewer)]
        public IActionResult Index()
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequirePermissions(Constants.Permission.MapCreator)]
        public override IActionResult Create(MapInputViewModel inputViewModel)
        {
            return base.Create(inputViewModel);
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public override IActionResult Edit(MapInputViewModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequirePermissions(Constants.Permission.MapRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.MapRemover)]
        public override IActionResult Delete(MapDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        #region Authors

        [HttpGet]
        [Route("edit/{key}/authors")]
        [RequirePermissions(Constants.Permission.MapEditor)]
        public ActionResult EditAuthors(System.Guid key)
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
        public IActionResult AddAuthor(System.Guid key, System.Guid authorId)
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
        public IActionResult DeleteAuthor(System.Guid key, System.Guid authorId)
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
        public ActionResult EditKeyWords(System.Guid key)
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
        public IActionResult AddKeyWord(System.Guid key, System.Guid keyWordId)
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
        public IActionResult DeleteKeyWord(System.Guid key, System.Guid keyWordId)
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
        public ActionResult EditImages(System.Guid key)
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
        public IActionResult AddImage(System.Guid key)
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
        public IActionResult DeleteImage(System.Guid key, System.Guid imageId)
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
        public ActionResult EditVideos(System.Guid key)
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
        public IActionResult AddVideo(System.Guid key, string about)
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
        public IActionResult DeleteVideo(System.Guid key, System.Guid videoId)
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
        public IActionResult EditVideo(System.Guid key, System.Guid videoId, string about)
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
        public ActionResult EditDocuments(System.Guid key)
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
        public IActionResult AddDocument(System.Guid key, DocumentInputViewModel model)
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
        public IActionResult RemoveDocument(System.Guid key, System.Guid documentId)
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
        public IActionResult EditDocument(System.Guid key, DocumentInputViewModel modelInput)
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
    }
}

