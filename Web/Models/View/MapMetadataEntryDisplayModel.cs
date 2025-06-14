namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class MapMetadataEntryDisplayModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Camaguey.Persistence.Entities.MapMetadataEntry, long>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public void Import(Camaguey.Persistence.Entities.MapMetadataEntry entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Value = entity.Value;
        }
    }
}
