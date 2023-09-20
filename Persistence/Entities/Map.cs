using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class Map : Framework.Guama.Core.Persistence.Entities.Contract.IEntity<System.Guid>
    {
        public Map()
        {
            Id = System.Guid.NewGuid();
            KeyWordBridges = new List<MapKeyWordBridge>();
            ImageBridges = new List<MapImageBridge>();
            VideoBridges = new List<MapVideoBridge>();
            DocumentBridges = new List<MapDocumentBridge>();
            AuthorBridges = new List<MapAuthorBridge>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public int Priority { get; set; } = 1;

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        public string QgisProjectPath { get; set; }

        public string Description { get; set; }

        public List<MapKeyWordBridge> KeyWordBridges { get; set; }

        public List<MapImageBridge> ImageBridges { get; set; }

        public List<MapVideoBridge> VideoBridges { get; set; }

        public List<MapDocumentBridge> DocumentBridges { get; set; }

        public List<MapAuthorBridge> AuthorBridges { get; set; }
    }
}
