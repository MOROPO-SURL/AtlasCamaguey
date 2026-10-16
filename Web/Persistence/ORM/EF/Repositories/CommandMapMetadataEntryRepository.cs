namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandMapMetadataEntryRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.MapMetadataEntry, long>
{
    public CommandMapMetadataEntryRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}