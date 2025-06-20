using Microsoft.EntityFrameworkCore;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories
{
    public class MapDocumentRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractEntityRepository<Camaguey.Persistence.Entities.MapDocument, System.Guid>
    {
        public MapDocumentRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
        {
        }

        protected override System.Linq.IQueryable<Camaguey.Persistence.Entities.MapDocument> GetQueryableDbSet()
        {
            return base.GetQueryableDbSet()
                .Include(entity => entity.Document)
                .Include(entity => entity.MetadataEntries);
        }
    }
}