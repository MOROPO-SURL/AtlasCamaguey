namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel(string key, Enumerations.InformationResourceKind? informationResourceKind)
        {
            Key = key;
            InformationResourceKind = informationResourceKind;
        }

        public string Key { get; private set; }
            
        public Enumerations.InformationResourceKind? InformationResourceKind { get; private set; }

        public SectionDisplayViewModel[] Sections { get; set; }

        public EpigraphDisplayViewModel[] Epigraphs { get; set; }

        public MapDisplayViewModel[] Maps { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }
    }
}
