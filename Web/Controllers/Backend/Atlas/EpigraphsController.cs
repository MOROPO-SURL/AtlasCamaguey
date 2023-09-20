using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class EpigraphsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Epigraph, System.Guid, EpigraphInputViewModel, EpigraphDisplayViewModel>
    {
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IImageFileManagerFacade imageFileManager;

        public EpigraphsController(IImageFileManagerFacade imageFileManager, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Section, System.Guid> sectionRepository, IEntityRepository<Epigraph, System.Guid> repository, IStringLocalizer<EpigraphsController> localizer, ILogger<EpigraphsController> logger) : base(repository, localizer, logger)
        {
            this.sectionRepository = sectionRepository;
            this.imageRepository = imageRepository;
            this.imageFileManager = imageFileManager;
        }

        [RequirePermissions(Constants.Permission.EpigraphViewer)]
        public IActionResult Index()
        {
            return View();
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

                var image = imageRepository.Add(new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Collaborator Persons " + entity.Id }, User.Identity.Name);

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

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public IActionResult EditMaps(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public IActionResult EditKeyWords(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public IActionResult EditImages(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditVideos(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public IActionResult EditDocuments(System.Guid key)
        {
            return View();
        }
    }
}

