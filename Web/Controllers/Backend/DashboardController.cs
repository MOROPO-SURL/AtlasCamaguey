using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Extensions;
using MOROPO.Framework.Guama.Core.Persistence.ORM.EF.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Security.Personal.Persistence.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend;

[Microsoft.AspNetCore.Authorization.Authorize]
[Framework.Guama.Core.Presentation.Web.Security.Attributes.RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
[Framework.Guama.Core.Presentation.Web.Security.Attributes.ChangeUserPasswordRequiredAction]
[Framework.Guama.Core.Presentation.Web.Security.Attributes.CatchChangeUserPasswordRequiredException]
[Route("[area]/[controller]")]
[Area("management")]
public class DashboardController : Framework.Guama.Core.Presentation.Web.Common.Controllers.BaseWebController
{
    private static readonly string[] DAYS_OF_WEEK = new[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

    private readonly IEnumerable<Framework.Guama.Core.Presentation.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders;

    private readonly IQueryEntityRepository<User, string> userRepository;
    private readonly IQueryEntityRepository<Trace, long> traceRepository;
    private readonly IQueryEntityRepository<Section, System.Guid> sectionRepository;
    private readonly IQueryEntityRepository<Epigraph, System.Guid> epigraphRepository;
    private readonly IQueryEntityRepository<Map, string> mapRepository;

    public DashboardController(
        IQueryEntityRepository<Map, string> mapRepository, 
        IQueryEntityRepository<Epigraph, System.Guid> epigraphRepository, 
        IQueryEntityRepository<Section, System.Guid> sectionRepository, 
        IQueryEntityRepository<Trace, long> traceRepository, 
        IQueryEntityRepository<User, string> userRepository, 
        IEnumerable<Framework.Guama.Core.Presentation.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders, 
        Microsoft.Extensions.Localization.IStringLocalizer<DashboardController> localizer, 
        ILogger<DashboardController> logger) : base(localizer, logger)
    {
        this.managementDashboardSectionProviders = managementDashboardSectionProviders;
        this.userRepository = userRepository;
        this.traceRepository = traceRepository;
        this.sectionRepository = sectionRepository;
        this.epigraphRepository = epigraphRepository;
        this.mapRepository = mapRepository;
    }

    public IActionResult Index()
    {
        var model = new Models.View.Backend.HomeViewModel
        {
            Sections = managementDashboardSectionProviders.ToManagementDashboardSectionGroups(),
            SectionCount = sectionRepository.Count(),
            EpigraphCount = epigraphRepository.Count(),
            MapCount = mapRepository.Count(),
            UserTotalCount = userRepository.Count(),
            UserDisabledCount = userRepository.Count(entity => !entity.Enabled),
            TraceTotalCount = traceRepository.Count(),
            UserActivityMap = new Dictionary<string, long>(),
            WeekTracesMap = new Dictionary<string, int>()
        };

        var traces = traceRepository.GetAll().ToArray();

        foreach (var trace in traces)
        {
            if (!model.UserActivityMap.ContainsKey(trace.Author))
            {
                model.UserActivityMap.Add(trace.Author, 1);
            }
            else
            {
                model.UserActivityMap[trace.Author]++;
            }
        }

        var startEdgeDate = System.DateTime.Today;
        var passWeekEdgeDate = startEdgeDate.AddDays(-6);

        for (System.DateTime currentDate = passWeekEdgeDate; currentDate <= startEdgeDate; currentDate = currentDate.AddDays(1))
        {
            var dailyCount = traceRepository.Count(entity => entity.Registered.Date == currentDate);

            model.TraceWeekCount += dailyCount;

            model.WeekTracesMap.Add(DAYS_OF_WEEK[(int)currentDate.DayOfWeek], dailyCount);
        }

        return View(model);
    }
}