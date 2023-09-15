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

    public class EpigraphController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Epigraph, System.Guid, EpigraphInputViewModel, EpigraphDisplayViewModel>
    {
        public EpigraphController(IEntityRepository<Epigraph, Guid> repository, IStringLocalizer<EpigraphController> localizer, ILogger<EpigraphController> logger) : base(repository, localizer, logger)
        {
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
        public override IActionResult Create(EpigraphInputViewModel inputViewModel)
        {
            return base.Create(inputViewModel);
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.EpigraphEditor)]
        public override IActionResult Edit(EpigraphInputViewModel inputModel)
        {
            return base.Edit(inputModel);
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
    }
}

