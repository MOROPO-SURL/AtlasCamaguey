namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class MapIndexViewModel
    {
        public MapDisplayViewModel Map { get; set; }

        public AuthorDisplayViewModel[] Authors { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }
    }
}
