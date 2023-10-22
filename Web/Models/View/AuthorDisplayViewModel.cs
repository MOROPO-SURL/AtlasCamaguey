namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class AuthorDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.Author, System.Guid>
    {
        public System.Guid Id { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string About { get; set; }

        public string NamePrefix { get; set; }

        public InstitutionDisplayViewModel Institution { get; set; }

        public void Import(Camaguey.Persistence.Entities.Author entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.ImageId;
            ImageExtension = entity.Identity.ImageId == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Identity.Image.ContentType);
            Name = entity.Identity.Name;
            LastName = entity.Identity.LastName;
            About = entity.Identity.About;
            NamePrefix = entity.NamePrefix;

            if (entity.InstitutionId != null)
            {
                Institution = new InstitutionDisplayViewModel();

                Institution.Import(entity.Institution);
            }
        }
    }
}
