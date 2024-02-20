using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class SectionRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.AbstractEntityRepository<Camaguey.Persistence.Entities.Section, System.Guid>
    {
        public SectionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Section> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet().Include(entity => entity.Image);
        }

        public override System.Func<Camaguey.Persistence.Entities.Section, object> GetPagingOrderSelector()
        {
            return entity => entity.Name;
        }
    }
}
