using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapImageBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Map, string, Image, System.Guid>
    {
        public string ProwId { get; set; }
        public Map Prow { get; set; }

        public System.Guid SternId { get; set; }
        public Image Stern { get; set; }
    }
}
