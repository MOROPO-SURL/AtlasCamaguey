using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Complements
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("complements")]
    public class KeyWordsController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<KeyWord, System.Guid, KeyWordInputViewModel, KeyWordDisplayViewModel>
	{
        public KeyWordsController(IEntityRepository<KeyWord, System.Guid> repository, IStringLocalizer<KeyWordsController> localizer, ILogger<KeyWordsController> logger) : base(repository, localizer, logger)
        {
        }

        [RequirePermissions(Constants.Permission.KeyWordViewer)]
        public IActionResult Index()
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.KeyWordCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequirePermissions(Constants.Permission.KeyWordCreator)]
        public override IActionResult Create(KeyWordInputViewModel inputViewModel)
        {
            return base.Create(inputViewModel);
        }

        [RequirePermissions(Constants.Permission.KeyWordEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.KeyWordEditor)]
        public override IActionResult Edit(KeyWordInputViewModel inputModel)
        {
            return base.Edit(inputModel);
        }

        [RequirePermissions(Constants.Permission.KeyWordRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.KeyWordRemover)]
        public override IActionResult Delete(KeyWordDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }
    }
}

