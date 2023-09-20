using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class InstitutionDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Institution, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Alias { get; set; }

        public void Import(Institution entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.ImageId;
            ImageExtension = entity.Identity.ImageId == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Identity.Image.ContentType);
            Name = entity.Identity.Name;
            Description = entity.Identity.About;
            Alias = entity.Alias;
        }
    }
}
