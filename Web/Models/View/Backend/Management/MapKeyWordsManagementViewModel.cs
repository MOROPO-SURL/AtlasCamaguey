namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Management
{
    internal sealed class MapKeyWordsManagementViewModel : Contract.AbstractKeyWordsManagementViewModel
    {
        public MapDisplayViewModel Map { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableKeyWordSelectListItems { get; set; }
    }
}
