using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Identity.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class AuthorsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Author, System.Guid, AuthorInputViewModel, AuthorDisplayViewModel>
    {
        private readonly IEntityRepository<Institution, System.Guid> institutionRepository;
        private readonly IEntityRepository<Image, System.Guid> imageRepository;
        private readonly IEntityRepository<PersonIdentity, System.Guid> identityRepository;
        private readonly IImageFileManagerFacade imageFileManager;

        public AuthorsController(IImageFileManagerFacade imageFileManager, IEntityRepository<PersonIdentity, System.Guid> identityRepository, IEntityRepository<Image, System.Guid> imageRepository, IEntityRepository<Institution, System.Guid> institutionRepository, IEntityRepository<Author, System.Guid> repository, Microsoft.Extensions.Localization.IStringLocalizer<AuthorsController> localizer, ILogger<AuthorsController> logger) : base(repository, localizer, logger)
        {
            this.institutionRepository = institutionRepository;
            this.imageRepository = imageRepository;
            this.identityRepository = identityRepository;
            this.imageFileManager = imageFileManager;
        }

        [RequirePermissions(Constants.Permission.AuthorViewer)]
        public IActionResult Index() => View();


        [RequirePermissions(Constants.Permission.AuthorCreator)]
        public override IActionResult Create() => base.Create();


        [RequirePermissions(Constants.Permission.AuthorCreator)]
        public override IActionResult Create(AuthorInputViewModel inputViewModel)
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

        [RequirePermissions(Constants.Permission.AuthorEditor)]
        public override IActionResult Edit(System.Guid key) => base.Edit(key);

        [RequirePermissions(Constants.Permission.AuthorEditor)]
        public override IActionResult Edit(AuthorInputViewModel inputViewModel)
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


        [RequirePermissions(Constants.Permission.AuthorRemover)]
        public override IActionResult Delete(System.Guid key) => base.Delete(key);

        [RequirePermissions(Constants.Permission.AuthorRemover)]
        public override IActionResult Delete(AuthorDisplayViewModel displayViewModel) => base.Delete(displayViewModel);

        protected override void PreCreate(Author entity, AuthorInputViewModel model)
        {
            base.PreCreate(entity, model);

            if (model.InstitutionId != null)
            {
                entity.Institution = institutionRepository.Get(model.InstitutionId.Value);
                entity.InstitutionId = entity.Institution.Id;
            }

            if (model.ImageFile == null) return;

            using (var memoryStream = new System.IO.MemoryStream())
            {
                model.ImageFile.CopyTo(memoryStream);

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Author " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Identity.Image = image;
            }
        }

        protected override void PreEdit(Author entity, AuthorInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (entity.InstitutionId != model.InstitutionId)
            {
                if (model.InstitutionId != null)
                {
                    entity.Institution = institutionRepository.Get(model.InstitutionId.Value);
                    entity.InstitutionId = entity.Institution.Id;
                }
                else
                {
                    entity.InstitutionId = null;
                    entity.Institution = null;
                }
            }

            if (model.InstitutionId != null)
            {
                entity.Institution = institutionRepository.Get(model.InstitutionId.Value);
                entity.InstitutionId = entity.Institution.Id;
            }

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

                var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Author " + entity.Id };

                imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

                entity.Identity.Image = image;

                imageRepository.Add(image, User.Identity.Name);
            }
        }

        protected override void PostDelete(Author entity)
        {
            base.PostDelete(entity);

            identityRepository.Delete(entity.Identity, User.Identity.Name);

            if (entity.Identity.Image == null) return;

            imageRepository.Delete(entity.Identity.Image, User.Identity.Name);

            imageFileManager.DeleteImageFile(entity.Identity.Image);
        }

        protected override void ConfigureCreate(AuthorInputViewModel model)
        {
            base.ConfigureCreate(model);

            Populate(model);
        }

        protected override void ConfigureEdit(Author entity, AuthorInputViewModel model)
        {
            base.ConfigureEdit(entity, model);

            Populate(model);
        }

        protected override void ConfigurePostFailCreateValidation(AuthorInputViewModel model)
        {
            base.ConfigurePostFailCreateValidation(model);

            Populate(model);
        }

        protected override void ConfigurePostFailEditValidation(Author entity, AuthorInputViewModel model)
        {
            base.ConfigurePostFailEditValidation(entity, model);

            Populate(model);
        }

        private void Populate(AuthorInputViewModel model)
        {
            model.InstitutionSelectListItems = institutionRepository.GetAll().Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = entity.Id.ToString(),
                Text = entity.Alias == null ? entity.Identity.Name : string.Format("{0} ({1})", entity.Identity.Name, entity.Alias),
            }).ToArray();
        }
    }
}
