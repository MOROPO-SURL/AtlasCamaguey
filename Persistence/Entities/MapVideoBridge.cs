using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapVideoBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Map, System.Guid, Video, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Map Prow { get; set; }

        public System.Guid SternId { get; set; }
        public Video Stern { get; set; }
    }
}
