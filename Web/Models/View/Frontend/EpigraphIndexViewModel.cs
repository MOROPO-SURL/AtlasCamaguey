namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class EpigraphIndexViewModel
    {
        public EpigraphDisplayViewModel Epigraph { get; set; }

        public MapDisplayViewModel[] Maps { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }
    }
}
