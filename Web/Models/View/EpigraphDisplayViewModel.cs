namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class EpigraphDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.Epigraph, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        public Camaguey.Persistence.Enumerations.EpigraphLayout Layout { get; set; }

        public virtual void Import(Camaguey.Persistence.Entities.Epigraph entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
            Layout = entity.Layout;
        }
    }
}
