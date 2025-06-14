namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities.Contract
{
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public abstract class AbstractMetadataEntry : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<long>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Value { get; set; }
    }
}
