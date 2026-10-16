using Microsoft.EntityFrameworkCore;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryInstitutionRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Institution, System.Guid>
{
    public QueryInstitutionRepository(DbContext dbContext) : base(dbContext)
    {
    }

    protected override System.Linq.IQueryable<Institution> GetQueryableDbSet()
    {
        return base.GetQueryableDbSet()
            .AsSplitQuery()
            .Include(entity => entity.Identity).ThenInclude(entity => entity.Image);
    }
}