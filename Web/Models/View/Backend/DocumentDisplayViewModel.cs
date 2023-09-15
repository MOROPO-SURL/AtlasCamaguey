using System;
using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class DocumentDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Document, System.Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string Description { get; set; }

        public long Length { get; set; }

        public void Import(Document entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Extension = MimeTypes.MimeTypeMap.GetExtension(entity.ContentType);
            Description = entity.Description;
            Length = entity.Length;
        }
    }
}

