using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
	public class Theme : Framework.Guama.Core.Persistence.Entities.Contract.INamedEntity<System.Guid>, Framework.Guama.Core.Persistence.Entities.Contract.IPriorizableEntity<System.Guid>
	{
		[System.ComponentModel.DataAnnotations.Key]
		public System.Guid Id { get; set; } = System.Guid.NewGuid();

		public string Name { get; set; }

		public int Priority { get; set; }

		[System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(EpigraphId))]
		public Epigraph Epigraph { get; set; }
		public System.Guid EpigraphId { get; set; }

		public List<ThemeMapBridge> MapBridges { get; set; } = new List<ThemeMapBridge>();
    }
}
