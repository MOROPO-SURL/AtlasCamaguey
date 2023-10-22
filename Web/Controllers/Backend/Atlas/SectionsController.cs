using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Backend.Controllers;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class SectionsController : AbstractEntityManagementController<Section, System.Guid, SectionInputViewModel, SectionDisplayViewModel>
    {
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IImageFileManagerFacade imageFileManager;

        public SectionsController(IImageFileManagerFacade imageFileManager, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Section, System.Guid> repository, IStringLocalizer<SectionsController> localizer, ILogger<SectionsController> logger) : base(repository, localizer, logger)
        {
            this.imageRepository = imageRepository;
            this.imageFileManager = imageFileManager;
        }

        [RequirePermissions(Constants.Permission.SectionViewer)]
        public IActionResult Index()
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.SectionCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequirePermissions(Constants.Permission.SectionCreator)]
        public override IActionResult Create(SectionInputViewModel model)
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

        [RequirePermissions(Constants.Permission.SectionEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.SectionEditor)]
        public override IActionResult Edit(SectionInputViewModel model)
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

        [RequirePermissions(Constants.Permission.SectionRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.SectionRemover)]
        public override IActionResult Delete(SectionDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(Section entity, SectionInputViewModel model)
        {
            base.PreCreate(entity, model);

            if (model.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Section " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Image = image;
            }
        }

        protected override void PreEdit(Section entity, SectionInputViewModel model)
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

        protected override void PostDelete(Section entity)
        {
            base.PostDelete(entity);

            if (entity.Image == null) return;

            imageRepository.Delete(entity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Image);
        }
    }
}