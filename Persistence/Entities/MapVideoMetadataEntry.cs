namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapVideoMetadataEntry : Contract.AbstractMetadataEntry
    {
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(MapVideoId))]
        public MapVideo MapVideo { get; set; }
        public System.Guid MapVideoId { get; set; }
    }
}
