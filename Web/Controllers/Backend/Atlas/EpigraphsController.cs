using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Display;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input;
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
    public class EpigraphsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<Epigraph, System.Guid, EpigraphInputViewModel, EpigraphDisplayViewModel>
    {
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<Document, System.Guid> documentRepository;
        private readonly IEntityRepository<Video, System.Guid> videoRepository;
        private readonly IEntityRepository<KeyWord, System.Guid> keyWordRepository;
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IDocumentFileManagerFacade documentFileManager;
        private readonly IVideoFileManagerFacade videoFileManager;

        public EpigraphsController(IVideoFileManagerFacade videoFileManager, IDocumentFileManagerFacade documentFileManager, IImageFileManagerFacade imageFileManager, IEntityRepository<KeyWord, System.Guid> keyWordRepository, IEntityRepository<Video, System.Guid> videoRepository, IEntityRepository<Document, System.Guid> documentRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Section, System.Guid> sectionRepository, IEntityRepository<Epigraph, System.Guid> repository, IStringLocalizer<EpigraphsController> localizer, ILogger<EpigraphsController> logger) : base(repository, localizer, logger)
        {
            this.sectionRepository = sectionRepository;
            this.imageRepository = imageRepository;
            this.documentRepository = documentRepository;
            this.videoRepository = videoRepository;
            this.keyWordRepository = keyWordRepository;
            this.imageFileManager = imageFileManager;
            this.documentFileManager = documentFileManager;
            this.videoFileManager = videoFileManager;
        }

        [RequirePermissions(Constants.Permission.EpigraphViewer)]
        public IActionResult Index()
        {
            return View();
        }

        protected override System.Func<Epigraph, object> GetOrderSelector()
        {
            return entity => entity.Name;
        }

        [RequirePermissions(Constants.Permission.EpigraphCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequirePermissions(Constants.Permission.EpigraphCreator)]
        public override IActionResult Create(EpigraphInputViewModel model)
        {
            if (Request.Form.Files.Count > 0)
            {
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    ModelState.ClearValidationState("ImageFile");

                    model.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(model);

                    updateModelTask.Wait();
                }
            }

            return base.Create(model);
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public override IActionResult Edit(EpigraphInputViewModel model)
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

        [RequirePermissions(Constants.Permission.EpigraphRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.EpigraphRemover)]
        public override IActionResult Delete(EpigraphDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(Epigraph entity, EpigraphInputViewModel model)
        {
            base.PreCreate(entity, model);

            entity.Section = sectionRepository.Get(model.SectionId.Value);
            entity.SectionId = entity.Section.Id;

            if (model.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Section " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Image = image;
            }
        }

        protected override void PreEdit(Epigraph entity, EpigraphInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (entity.SectionId != model.SectionId)
            {
                entity.Section = sectionRepository.Get(model.SectionId.Value);
                entity.SectionId = entity.Section.Id;
            }

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

                var image = imageRepository.Add(new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Epigraph " + entity.Id }, User.Identity.Name);

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

        protected override void PostDelete(Epigraph entity)
        {
            base.PostDelete(entity);

            if (entity.Image == null) return;

            imageRepository.Delete(entity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Image);
        }

        protected override void ConfigureCreate(EpigraphInputViewModel model)
        {
            base.ConfigureCreate(model);

            Populate(model);
        }

        protected override void ConfigureEdit(Epigraph entity, EpigraphInputViewModel model)
        {
            base.ConfigureEdit(entity, model);

            Populate(model);
        }

        protected override void ConfigurePostFailCreateValidation(EpigraphInputViewModel model)
        {
            base.ConfigurePostFailCreateValidation(model);

            Populate(model);
        }

        protected override void ConfigurePostFailEditValidation(Epigraph entity, EpigraphInputViewModel model)
        {
            base.ConfigurePostFailEditValidation(entity, model);

            Populate(model);
        }

        private void Populate(EpigraphInputViewModel model)
        {
            model.SectionSelectListItems = sectionRepository.GetAll().OrderBy(entity => entity.Name).Select(entity =>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = entity.Id.ToString(),
                Text = entity.Name
            }).ToArray();
        }

        #region KeyWords

        [HttpGet]
        [Route("edit/{key}/keywords")]
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public ActionResult EditKeyWords(System.Guid key)
        {
            var entity = Repository.Get(key);

            var releatedKeyWords = entity.KeyWordBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new Models.View.Backend.Management.EpigraphKeyWordsManagementViewModel
            {
                Epigraph = new EpigraphDisplayViewModel(),
                KeyWords = entity.KeyWordBridges.Select(bridge =>
                {
                    var result = new Models.View.KeyWordDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableKeyWordSelectListItems = keyWordRepository.GetAll().ToArray().Except(releatedKeyWords).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = entity.Id.ToString(),
                    Text = entity.Name
                }).ToArray()
            };

            model.Epigraph.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/keywords/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public IActionResult AddKeyWord(System.Guid key, System.Guid keyWordId)
        {
            try
            {
                var entity = Repository.Get(key);

                if (keyWordId != default)
                {
                    try
                    {
                        entity.KeyWordBridges.Add(new EpigraphKeyWordBridge
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public ActionResult EditImages(System.Guid key)
        {
            var entity = Repository.Get(key);

            var model = new Models.View.Backend.Management.EpigraphImagesManagementViewModel
            {
                Epigraph = new EpigraphDisplayViewModel(),
                Images = entity.ImageBridges.Select(bridge =>
                {
                    var result = new Models.View.ImageDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray()
            };

            model.Epigraph.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/images/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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

                            var image = new Image { ContentType = imageFile.ContentType, Length = imageFile.Length, About = "Epigraph Compl. " + entity.Id };

                            imageRepository.Add(image, User.Identity.Name, false);

                            imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), imageFile.ContentType);

                            entity.ImageBridges.Add(new EpigraphImageBridge
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public ActionResult EditVideos(System.Guid key)
        {
            var entity = Repository.Get(key);

            var model = new Models.View.Backend.Management.EpigraphVideosManagementViewModel
            {
                Epigraph = new EpigraphDisplayViewModel(),
                Videos = entity.VideoBridges.Select(bridge =>
                {
                    var result = new Models.View.VideoDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray()
            };

            model.Epigraph.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/videos/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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

                            entity.VideoBridges.Add(new EpigraphVideoBridge
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public ActionResult EditDocuments(System.Guid key)
        {
            var entity = Repository.Get(key);

            var model = new Models.View.Backend.Management.EpigraphDocumentsManagementViewModel
            {
                Epigraph = new EpigraphDisplayViewModel(),
                Documents = entity.DocumentBridges.Select(item =>
                {
                    var result = new Models.View.DocumentDisplayViewModel();

                    result.Import(item.Stern);

                    return result;
                }).ToArray()
            };

            model.Epigraph.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/documents/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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

                            entity.DocumentBridges.Add(new EpigraphDocumentBridge
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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
        [RequirePermissions(Constants.Permission.EpigraphEditor)]
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