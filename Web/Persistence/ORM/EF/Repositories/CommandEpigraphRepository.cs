using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandEpigraphRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.Epigraph, System.Guid>
{
    public CommandEpigraphRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Epigraph> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.KeyWordBridges).ThenInclude(bridge => bridge.Stern)
            .Include(entity => entity.ImageBridges).ThenInclude(bridge => bridge.Stern)
            .Include(entity => entity.VideoBridges).ThenInclude(bridge => bridge.Stern)
            .Include(entity => entity.DocumentBridges).ThenInclude(bridge => bridge.Stern);
    }
}