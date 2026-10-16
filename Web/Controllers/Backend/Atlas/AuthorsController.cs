using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;
using MOROPO.Framework.Guama.Identity.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas;

[Microsoft.AspNetCore.Authorization.Authorize]
[RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
[Area("atlas")]
public class AuthorsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<Author, System.Guid, AuthorInputViewModel, AuthorDisplayViewModel>
{
    private readonly IQueryEntityRepository<Institution, System.Guid> queryInstitutionRepository;
    private readonly ICommandEntityRepository<Image, System.Guid> commandImageRepository;
    private readonly ICommandEntityRepository<PersonIdentity, System.Guid> commandIdentityRepository;
    private readonly IImageFileManagerFacade imageFileManager;

    public AuthorsController(
        IImageFileManagerFacade imageFileManager, 
        ICommandEntityRepository<Image, System.Guid> commandImageRepository, 
        ICommandEntityRepository<PersonIdentity, System.Guid> commandIdentityRepository,
        IQueryEntityRepository<Institution, System.Guid> queryInstitutionRepository,
        ICommandEntityRepository<Author, System.Guid> commandRepository, 
        IQueryEntityRepository<Author, System.Guid> queryRepository,
        Microsoft.Extensions.Localization.IStringLocalizer<AuthorsController> localizer, 
        ILogger<AuthorsController> logger) 
        : base(commandRepository, queryRepository, localizer, logger)
    {
        this.queryInstitutionRepository = queryInstitutionRepository;
        this.commandImageRepository = commandImageRepository;
        this.commandIdentityRepository = commandIdentityRepository;
        this.imageFileManager = imageFileManager;
    }

    [RequirePermissions(Constants.Permission.AuthorViewer)]
    public IActionResult Index() => View();

    protected override System.Func<Author, object> GetOrderSelector()
    {
        return entity => entity.Identity.Name;
    }

    [RequirePermissions(Constants.Permission.AuthorCreator)]
    public override IActionResult Create() => base.Create();


    [RequirePermissions(Constants.Permission.AuthorCreator)]
    public override IActionResult Create(AuthorInputViewModel model)
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
            entity.Institution = null;
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
                entity.Institution = null;
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
            entity.Institution = null;
            entity.InstitutionId = entity.Institution.Id;
        }

        if (model.ImageFile == null)
        {
            if (model.ResetImage && entity.Identity.Image != null)
            {
                imageFileManager.DeleteImageFile(entity.Identity.Image);
                commandImageRepository.Delete(entity.Identity.Image, User.Identity.Name);
            }
            return;
        }

        if (entity.Identity.Image != null)
        {
            imageFileManager.DeleteImageFile(entity.Identity.Image);

            commandImageRepository.Delete(entity.Identity.Image, User.Identity.Name);
        }

        using (var memoryStream = new System.IO.MemoryStream())
        {
            model.ImageFile.CopyTo(memoryStream);

            var image = new Image { ContentType = model.ImageFile.ContentType, Length = model.ImageFile.Length, About = "Author " + entity.Id };

            imageFileManager.SaveImageFileContent(image.Id, memoryStream.ToArray(), model.ImageFile.ContentType);

            entity.Identity.Image = image;

            commandImageRepository.Add(image, User.Identity.Name);
        }
    }

    protected override void PostDelete(Author entity)
    {
        base.PostDelete(entity);

        commandIdentityRepository.Delete(entity.Identity, User.Identity.Name);

        if (entity.Identity.Image == null) return;

        commandImageRepository.Delete(entity.Identity.Image, User.Identity.Name);

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
        model.InstitutionSelectListItems = [.. queryInstitutionRepository.GetAll().Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = entity.Id.ToString(),
            Text = entity.Alias == null ? entity.Identity.Name : string.Format("{0} ({1})", entity.Identity.Name, entity.Alias),
        })];
    }
}