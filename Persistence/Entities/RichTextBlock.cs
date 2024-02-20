namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class RichTextBlock : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<Enumerations.RichTextBlockKind>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public Enumerations.RichTextBlockKind Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Content { get; set; }
    }
}
