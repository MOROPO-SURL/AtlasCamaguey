using Microsoft.EntityFrameworkCore;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class InstitutionRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.AbstractEntityRepository<Institution, System.Guid>
    {
        public InstitutionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Institution, object> GetPagingOrderSelector()
        {
            return entity => entity.Identity.Name;
        }

        protected override System.Linq.IQueryable<Institution> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Image);
        }
    }
}
