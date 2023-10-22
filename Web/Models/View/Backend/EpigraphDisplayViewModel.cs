using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class EpigraphDisplayViewModel : View.EpigraphDisplayViewModel
    {
        public SectionDisplayViewModel Section { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }

        public MapDisplayViewModel[] Maps { get; set; }

        public override void Import(Epigraph entity)
        {
            base.Import(entity);

            Section = new SectionDisplayViewModel();
            Section.Import(entity.Section);

            KeyWords = entity.KeyWordBridges.Select(bridge =>
            {
                var model = new KeyWordDisplayViewModel();

                model.Import(bridge.Stern);

                return model;
            }).ToArray();

            Images = entity.ImageBridges.Select(bridge =>
            {
                var model = new ImageDisplayViewModel();

                model.Import(bridge.Stern);

                return model;
            }).ToArray();

            Videos = entity.VideoBridges.Select(bridge =>
            {
                var model = new VideoDisplayViewModel();

                model.Import(bridge.Stern);

                return model;
            }).ToArray();

            Documents = entity.DocumentBridges.Select(bridge =>
            {
                var model = new DocumentDisplayViewModel();

                model.Import(bridge.Stern);

                return model;
            }).ToArray();

            Maps = entity.MapBridges.Select(bridge =>
            {
                var model = new MapDisplayViewModel();

                model.Import(bridge.Stern);

                return model;
            }).ToArray();
        }
    }
}

