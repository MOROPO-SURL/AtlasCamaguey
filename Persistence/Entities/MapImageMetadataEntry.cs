namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapImageMetadataEntry : Contract.AbstractMetadataEntry
    {
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(MapImageId))]
        public MapImage MapImage { get; set; }
        public System.Guid MapImageId { get; set; }
    }
}
