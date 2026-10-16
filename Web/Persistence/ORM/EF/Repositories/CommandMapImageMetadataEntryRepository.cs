namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Repositories;

public class CommandMapImageMetadataEntryRepository : Framework.Guama.Core.Persistence.ORM.EF.Repositories.Contract.AbstractCommandEntityRepository<Camaguey.Persistence.Entities.MapImageMetadataEntry, long>
{
    public CommandMapImageMetadataEntryRepository(Microsoft.EntityFrameworkCore.DbContext dbContext) : base(dbContext)
    {
    }
}