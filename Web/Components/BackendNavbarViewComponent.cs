using Microsoft.AspNetCore.Mvc;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Extensions;
using MOROPO.Framework.Guama.Core.Presentation.Web.Backend.Providers.Facade;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Components
{
	public class BackendNavbarViewComponent : ViewComponent
    {
        private readonly System.Collections.Generic.IEnumerable<IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders;

        public BackendNavbarViewComponent(System.Collections.Generic.IEnumerable<IManagementDashboardSectionProviderFacade> managementDashboardSectionProviders)
        {
            this.managementDashboardSectionProviders = managementDashboardSectionProviders;
        }

        public IViewComponentResult Invoke()
        {
            return View(managementDashboardSectionProviders.ToManagementDashboardSectionGroups());
        }
    }
}

