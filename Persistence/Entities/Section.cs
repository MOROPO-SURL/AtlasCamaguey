namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Section : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        public Section()
        {
            Id = System.Guid.NewGuid();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public int Priority { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Description { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ImageId")]
        public Framework.Guama.Media.Persistence.Entities.Image Image { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public System.Guid? ImageId { get; set; }
    }
}
