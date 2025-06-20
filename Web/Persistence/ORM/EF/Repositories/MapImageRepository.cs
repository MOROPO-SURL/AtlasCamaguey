using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class MapImageRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.MapImage, System.Guid>
    {
        public MapImageRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.MapImage> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Image)
                .Include(entity => entity.MetadataEntries);
        }
    }
}
