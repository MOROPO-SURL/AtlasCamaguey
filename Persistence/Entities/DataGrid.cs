namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class DataGrid : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; } = System.Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }
    }
}
