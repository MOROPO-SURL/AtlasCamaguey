namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class MapIndexViewModel
    {
        public string QgisServerBaseUri { get; set; }

        public string QgisProjectsRootPath { get; set; }

        public MapDisplayViewModel Current { get; set; }

        public EpigraphDisplayViewModel Epigraph { get; set; }

        public SectionDisplayViewModel Section { get; set; }

        public SectionDisplayViewModel[] Sections { get; set; }

        public AuthorDisplayViewModel[] Authors { get; set; }

        public ImageDisplayViewModel[] Graphs { get; set; }

        public ImageDisplayViewModel[] Pictures { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }
    }
}
