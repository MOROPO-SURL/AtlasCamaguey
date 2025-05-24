namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class LayerRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.Layer, System.Guid>
    {
        public LayerRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }
    }
}
