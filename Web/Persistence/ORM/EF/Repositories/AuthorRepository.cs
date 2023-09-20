using Microsoft.EntityFrameworkCore;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class AuthorRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.AbstractEntityRepository<Author, System.Guid>
    {
        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public override System.Func<Author, object> GetPagingOrderSelector()
        {
            return entity => entity.Identity.Name;
        }

        protected override System.Linq.IQueryable<Author> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image);
        }
    }
}
