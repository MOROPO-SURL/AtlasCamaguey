namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandAuthorRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.Author, System.Guid>
{
    public CommandAuthorRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}