namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class RichTextBlockInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind>
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Id")]
        public Camaguey.Persistence.Enumerations.RichTextBlockKind Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Display(Name = "Text")]
        public string Content { get; set; }

        public Camaguey.Persistence.Entities.RichTextBlock Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Camaguey.Persistence.Entities.RichTextBlock entity)
        {
            Id = entity.Id;
            Content = entity.Content;
        }

        public void Mergue(Camaguey.Persistence.Entities.RichTextBlock entity)
        {
            entity.Content = Content;
        }
    }
}
