using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class AuthorRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.Author, System.Guid>
    {
        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Author> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image);
        }
    }
}
