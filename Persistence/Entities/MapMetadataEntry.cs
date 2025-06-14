namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapMetadataEntry : Contract.AbstractMetadataEntry
    {
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(MapId))]
        public Map Map { get; set; }
        public string MapId { get; set; }
    }
}
