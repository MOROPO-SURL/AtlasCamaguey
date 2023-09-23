namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    internal sealed class EpigraphMapsManagementViewModel
    {
        public MapDisplayViewModel[] Maps { get; set; }

        public EpigraphDisplayViewModel Epigraph { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableMapSelectListItems { get; set; }
    }
}
