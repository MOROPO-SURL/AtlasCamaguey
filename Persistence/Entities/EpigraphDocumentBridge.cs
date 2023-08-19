using MOROPO.Framework.Guama.Media.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class EpigraphDocumentBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Epigraph, System.Guid, Document, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Epigraph Prow { get; set; }

        public System.Guid SternId { get; set; }
        public Document Stern { get; set; }
    }
}
