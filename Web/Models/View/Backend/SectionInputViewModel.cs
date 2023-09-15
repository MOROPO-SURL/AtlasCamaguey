using System;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class SectionInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Section, System.Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }

        public Section Export()
        {
            var entity = new Section();

            Mergue(entity);

            return entity;
        }

        public void Import(Section entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
        }

        public void Mergue(Section entity)
        {
            entity.Name = Name;
            entity.Description = Description;

        }
    }
}

