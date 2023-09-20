using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class MapsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<Map, System.Guid, MapInputViewModel, MapDisplayViewModel>
    {
        public MapsController(IEntityRepository<Map, System.Guid> repository, IStringLocalizer<MapsController> localizer, ILogger<MapsController> logger) : base(repository, localizer, logger)
        {
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

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditAuthors(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditKeyWords(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditImages(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditVideos(System.Guid key)
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.MapEditor)]
        public IActionResult EditDocuments(System.Guid key)
        {
            return View();
        }
    }
}

