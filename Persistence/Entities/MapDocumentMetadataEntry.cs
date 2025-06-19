namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapDocumentMetadataEntry : Contract.AbstractMetadataEntry
    {
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(MapDocumentId))]
        public MapDocument MapDocument { get; set; }
        public System.Guid MapDocumentId { get; set; }
    }
}
