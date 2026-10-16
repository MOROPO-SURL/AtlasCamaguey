namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryRichTextBlockRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind>
{
    public QueryRichTextBlockRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}