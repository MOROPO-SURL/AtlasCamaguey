using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Backend.Controllers;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Complements
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("complements")]

    public class SectionsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Section, System.Guid, SectionInputViewModel, SectionDisplayViewModel>
    {
        public SectionsController(IEntityRepository<Section, Guid> repository, IStringLocalizer<SectionsController> localizer, ILogger<SectionsController> logger) : base(repository, localizer, logger)
        {
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
        public override IActionResult Create(SectionInputViewModel inputViewModel)
        {
            return base.Create(inputViewModel);
        }

        [RequirePermissions(Constants.Permission.SectionEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.SectionEditor)]
        public override IActionResult Edit(SectionInputViewModel inputModel)
        {
            return base.Edit(inputModel);
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
    }
}

