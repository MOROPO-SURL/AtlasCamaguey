using Microsoft.AspNetCore.Mvc;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Display;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input;
using MOROPO.Framework.Guama.Core.Persistence.ORM.EF.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Security
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("security")]
    public class TracesController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementFilteredController<Trace, long, TraceInputViewModel, TraceDisplayViewModel, Models.View.Backend.Filter.TracesFilterViewModel>
    {
        public TracesController(Framework.Guama.Core.Persistence.Repositories.IEntityRepository<Trace, long> repository, Microsoft.Extensions.Localization.IStringLocalizer<TracesController> localizer, Microsoft.Extensions.Logging.ILogger<TracesController> logger) : base(repository, localizer, logger)
        {
        }

        [RequirePermissions(Constants.Permission.TraceViewer)]
        public IActionResult Index()
        {
            return View();
        }

        [RequirePermissions(Constants.Permission.TraceViewer)]
        public override IActionResult Filter(Models.View.Backend.Filter.TracesFilterViewModel filterModel)
        {
            return base.Filter(filterModel);
        }

        protected override string GetFilterSessionID()
        {
            return "traces-management-filter";
        }

        public override IActionResult Create()
        {
            return NotFound();
        }

        public override IActionResult Create(TraceInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Edit(long key)
        {
            return NotFound();
        }

        public override IActionResult Edit(TraceInputViewModel modelInput)
        {
            return NotFound();
        }

        public override IActionResult Delete(long key)
        {
            return NotFound();
        }

        public override IActionResult Delete(TraceDisplayViewModel displayModel)
        {
            return NotFound();
        }
    }
}
