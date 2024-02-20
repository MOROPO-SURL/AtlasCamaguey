using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Enumerations;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Content
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("content")]
    public class RichTextBlocksController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.AbstractEntityManagementController<RichTextBlock, RichTextBlockKind, RichTextBlockInputViewModel, RichTextBlockDisplayViewModel>
    {
        public RichTextBlocksController(Framework.Guama.Core.Persistence.Repositories.IEntityRepository<RichTextBlock, RichTextBlockKind> repository, Microsoft.Extensions.Localization.IStringLocalizer<RichTextBlocksController> localizer, ILogger<RichTextBlocksController> logger) : base(repository, localizer, logger)
        {
        }

        [RequirePermissions(Constants.Permission.RichTextBlockViewer)]
        public override IActionResult Index(int sheet = 1, int limit = 25)
        {
            return base.Index(sheet, limit);
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(RichTextBlockInputViewModel modelInput)
        {
            return NotFound();
        }

        [RequirePermissions(Constants.Permission.RichTextBlockEditor)]
        public override IActionResult Edit(RichTextBlockKind key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.RichTextBlockEditor)]
        public override IActionResult Edit(RichTextBlockInputViewModel modelInput)
        {
            return base.Edit(modelInput);
        }

        public override IActionResult Delete(RichTextBlockKind key)
        {
            return NotFound();
        }

        public override IActionResult Delete(RichTextBlockDisplayViewModel displayModel)
        {
            return NotFound();
        }
    }
}
