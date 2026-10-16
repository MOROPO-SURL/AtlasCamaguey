using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandMapDocumentRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.MapDocument, System.Guid>
{
    public CommandMapDocumentRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.MapDocument> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.MetadataEntries);
    }
}