namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapDocumentEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractEntityTypeConfigurator<Camaguey.Persistence.Entities.MapDocument>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Camaguey.Persistence.Entities.MapDocument> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasOne(entity => entity.Document).WithOne().HasForeignKey<Camaguey.Persistence.Entities.MapDocument>(entity => entity.DocumentId).IsRequired().OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.ClientCascade);
        }
    }
}
