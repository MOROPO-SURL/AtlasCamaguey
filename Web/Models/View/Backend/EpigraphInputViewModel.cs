using System;
using System.Linq;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Enumerations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class EpigraphInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Epigraph, System.Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        public EpigraphLayout Layout { get; set; }

        public SectionDisplayViewModel Section { get; set; }

        public MapDisplayViewModel Map { get; set; }

        public KeyWordDisplayViewModel[] KeyWords { get; set; }

        public ImageDisplayViewModel[] Images { get; set; }

        public VideoDisplayViewModel[] Videos { get; set; }

        public DocumentDisplayViewModel[] Documents { get; set; }


        public Epigraph Export()
        {
            var entity = new Epigraph();

            Mergue(entity);

            return entity;
        }

        public void Import(Epigraph entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
            Layout = entity.Layout;

            Section = new SectionDisplayViewModel();
            Section.Import(entity.Section);

            Map = new MapDisplayViewModel();
            Map.Import(entity.Map);


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
        }

        public void Mergue(Epigraph entity)
        {
            entity.Name = Name;
            entity.Description = Description;
            entity.Layout = Layout;
        }
    }
}

