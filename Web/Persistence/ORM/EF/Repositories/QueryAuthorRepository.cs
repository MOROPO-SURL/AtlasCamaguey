using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryAuthorRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Camaguey.Persistence.Entities.Author, System.Guid>
{
    public QueryAuthorRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.Author> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.Identity).ThenInclude(entity => entity.Image)
            .Include(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image);
    }
}