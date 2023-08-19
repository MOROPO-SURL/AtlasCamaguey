using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class EpigraphImageBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Epigraph, System.Guid, Image, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Epigraph Prow { get; set; }
        
        public System.Guid SternId { get; set; }
        public Image Stern { get; set; }
    }
}