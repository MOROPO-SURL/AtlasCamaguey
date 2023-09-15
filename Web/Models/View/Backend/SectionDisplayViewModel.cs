using System;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Extensions;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class SectionDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Section, System.Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        public void Import(Section entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
        }
    }
}

