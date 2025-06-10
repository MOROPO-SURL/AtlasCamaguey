namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Management
{
    internal sealed class MapAuthorsManagementViewModel
    {
        public AuthorDisplayViewModel[] Authors { get; set; }

        public MapDisplayViewModel Map { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableAuthorSelectListItems { get; set; }
    }
}
