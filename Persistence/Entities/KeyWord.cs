namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class KeyWord : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; } = System.Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
    }
}
