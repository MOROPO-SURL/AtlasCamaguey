namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class EpigraphIndexViewModel
    {
        public EpigraphDisplayViewModel Current { get; set; }

        public SectionDisplayViewModel Section { get; set; }

        public SectionDisplayViewModel[] Sections { get; set; }

        public ThemeDisplayViewModel[] Themes { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }
    }
}
