using System;
using System.Xml.Linq;
using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class ImageDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Image, System.Guid>
    {
        public Guid Id { get; set ; }

        public string Extension { get; set; }

        public long Length { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public void Import(Image entity)
        {
            Id = entity.Id;
            Extension = MimeTypes.MimeTypeMap.GetExtension(entity.ContentType);
            Length = entity.Length;
            Height = entity.Height;
            Width = entity.Width;
        }
    }
}

