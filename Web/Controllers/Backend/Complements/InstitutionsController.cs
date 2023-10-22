using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Identity.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Complements
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("complements")]
    public class InstitutionsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Institution, System.Guid, InstitutionInputViewModel, InstitutionDisplayViewModel>
    {
        private readonly IImageFileManagerFacade imageFileManager;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<OrganizationIdentity, System.Guid> identityRepository;

        public InstitutionsController(IEntityRepository<Institution, System.Guid> repository,
            IStringLocalizer<InstitutionsController> localizer,
            ILogger<InstitutionsController> logger,
            IImageFileManagerFacade imageFileManagerFacade,
            IEntityRepository<Image, System.Guid> imageRepository,
            IEntityRepository<OrganizationIdentity, System.Guid> identityRepository
            ) : base(repository, localizer, logger)
        {
            imageFileManager = imageFileManagerFacade;
            this.imageRepository = imageRepository;
            this.identityRepository = identityRepository;
        }

        [RequirePermissions(Constants.Permission.InstitutionViewer)]
        public IActionResult Index() => View();


        [RequirePermissions(Constants.Permission.InstitutionCreator)]
        public override IActionResult Create() => base.Create();


        [RequirePermissions(Constants.Permission.InstitutionCreator)]
        public override IActionResult Create(InstitutionInputViewModel inputViewModel)
        {
            if (Request.Form.Files.Count > 0)
            {
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    ModelState.ClearValidationState("ImageFile");

                    inputViewModel.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(inputViewModel);

                    updateModelTask.Wait();
                }
            }

            return base.Create(inputViewModel);
        }

        [RequirePermissions(Constants.Permission.InstitutionEditor)]
        public override IActionResult Edit(System.Guid key) => base.Edit(key);

        [RequirePermissions(Constants.Permission.InstitutionEditor)]
        public override IActionResult Edit(InstitutionInputViewModel inputViewModel)
        {
            ModelState.ClearValidationState("ImageFile");
            ModelState.MarkFieldValid("ImageFile");

            if (Request.Form.Files.Count > 0)
            {
                var formFile = Request.Form.Files[0];

                if (formFile.Length > 0)
                {
                    inputViewModel.ImageFile = formFile;

                    var updateModelTask = TryUpdateModelAsync(inputViewModel);

                    updateModelTask.Wait();

                }
            }
            return base.Edit(inputViewModel);
        }


        [RequirePermissions(Constants.Permission.InstitutionRemover)]
        public override IActionResult Delete(System.Guid key) => base.Delete(key);

        [RequirePermissions(Constants.Permission.InstitutionRemover)]
        public override IActionResult Delete(InstitutionDisplayViewModel displayViewModel) => base.Delete(displayViewModel);

        protected override void PreCreate(Institution entity, InstitutionInputViewModel model)
        {
            base.PreCreate(entity, model);

            if (model.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Institution " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PreEdit(Institution entity, InstitutionInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (model.ImageFile == null)
            {
                if (model.ResetImage && entity.Identity.Image != null)
                {
                    imageFileManager.DeleteImageFile(entity.Identity.Image);
                    imageRepository.Delete(entity.Identity.Image, User.Identity.Name);
                }
                return;
            }

            if (entity.Identity.Image != null)
            {
                imageFileManager.DeleteImageFile(entity.Identity.Image);

                imageRepository.Delete(entity.Identity.Image, User.Identity.Name);
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Institution " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Identity.Image = image;

                imageRepository.Add(image, User.Identity.Name);
            }
        }

        protected override void PostDelete(Institution entity)
        {
            base.PostDelete(entity);

            identityRepository.Delete(entity.Identity, User.Identity.Name);

            if (entity.Identity.Image == null) return;

            imageRepository.Delete(entity.Identity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Identity.Image);
        }
    }
}
