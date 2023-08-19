namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class MapRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.AbstractEntityRepository<Camaguey.Persistence.Entities.Map, System.Guid>
    {
        public MapRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }
    }
}
