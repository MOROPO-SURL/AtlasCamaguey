using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryMapDocumentRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Camaguey.Persistence.Entities.MapDocument, System.Guid>
{
    public QueryMapDocumentRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.MapDocument> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.Document)
            .Include(entity => entity.MetadataEntries);
    }
}