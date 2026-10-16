namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandRichTextBlockRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind>
{
    public CommandRichTextBlockRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}