using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryThemeRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Camaguey.Persistence.Entities.Theme, System.Guid>
{
    public QueryThemeRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Theme> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.Epigraph)
            .Include(entity => entity.MapBridges).ThenInclude(bridge => bridge.Stern).ThenInclude(entity => entity.Image);
    }
}