namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Management
{
    internal sealed class ThemeMapsManagementViewModel
    {
        public MapDisplayViewModel[] Maps { get; set; }

        public ThemeDisplayViewModel Theme { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableMapSelectListItems { get; set; }
    }
}
