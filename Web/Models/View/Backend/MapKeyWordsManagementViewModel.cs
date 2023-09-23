namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    internal sealed class MapKeyWordsManagementViewModel : Contract.AbstractKeyWordsManagementViewModel
    {
        public MapDisplayViewModel Map { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableKeyWordSelectListItems { get; set; }
    }
}
