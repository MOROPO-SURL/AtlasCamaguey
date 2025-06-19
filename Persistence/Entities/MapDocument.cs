using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapDocument : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; } = System.Guid.NewGuid();

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(MapId))]
        public Map Map { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string MapId { get; set; }

        public Document Document { get; set; }
        public System.Guid DocumentId { get; set; }

    }
}
