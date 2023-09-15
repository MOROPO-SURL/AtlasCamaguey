namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class KeyWord : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>
    {
        public KeyWord()
        {
            Id = System.Guid.NewGuid();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
    }
}
