namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Management
{
    internal sealed class EpigraphKeyWordsManagementViewModel : Contract.AbstractKeyWordsManagementViewModel
    {
        public EpigraphDisplayViewModel Epigraph { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] AvailableKeyWordSelectListItems { get; set; }
    }
}
