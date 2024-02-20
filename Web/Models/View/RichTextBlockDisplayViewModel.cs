namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class RichTextBlockDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind>
    {
        public Camaguey.Persistence.Enumerations.RichTextBlockKind Id { get; set; }

        public string Content { get; set; }

        public void Import(Camaguey.Persistence.Entities.RichTextBlock entity)
        {
            Id = entity.Id;
            Content = entity.Content;
        }
    }
}
