using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class MapRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.Map, string>
    {
        public MapRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Map> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Image)
                .Include(entity => entity.Layers)
                .Include(entity => entity.AuthorBridges).ThenInclude(bridge => bridge.Stern).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(entity => entity.AuthorBridges).ThenInclude(bridge => bridge.Stern).ThenInclude(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(entity => entity.KeyWordBridges).ThenInclude(bridge => bridge.Stern)
                .Include(entity => entity.MetadataEntries)
                .Include(entity => entity.Images).ThenInclude(entity => entity.Image)
                .Include(entity => entity.Videos).ThenInclude(entity => entity.Video)
                .Include(entity => entity.Documents).ThenInclude(entity => entity.Document);
        }
    }
}
