namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Chart : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }

        public Enumerations.ChartKind Kind { get; set; }
    }
}
