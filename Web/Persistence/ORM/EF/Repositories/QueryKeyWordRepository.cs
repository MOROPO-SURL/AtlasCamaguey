namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class QueryKeyWordRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractQueryEntityRepository<Camaguey.Persistence.Entities.KeyWord, System.Guid>
{
    public QueryKeyWordRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}
