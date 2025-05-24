using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class EpigraphRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.Epigraph, System.Guid>
    {
        public EpigraphRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Epigraph> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Image)
                .Include(entity => entity.Section)
                .Include(entity => entity.MapBridges).ThenInclude(bridge => bridge.Stern).ThenInclude(entity => entity.Image)
                .Include(entity => entity.KeyWordBridges).ThenInclude(bridge => bridge.Stern)
                .Include(entity => entity.ImageBridges).ThenInclude(bridge => bridge.Stern)
                .Include(entity => entity.VideoBridges).ThenInclude(bridge => bridge.Stern)
                .Include(entity => entity.DocumentBridges).ThenInclude(bridge => bridge.Stern);
        }
    }
}
