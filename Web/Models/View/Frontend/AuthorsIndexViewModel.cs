namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class AuthorsIndexViewModel
    {
        public string Intro { get; set; }

        public AuthorOverviewViewModel[] AuthorOverviews { get; set; }

        public InstitutionDisplayViewModel[] Institutions { get; set; }

        public class AuthorOverviewViewModel
        {
            public AuthorDisplayViewModel Author { get; set; }

            public MapDisplayViewModel[] Maps { get; set; }
        }
    }
}
