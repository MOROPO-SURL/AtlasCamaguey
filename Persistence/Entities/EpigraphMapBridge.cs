namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class EpigraphMapBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Epigraph, System.Guid, Map, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Epigraph Prow { get; set; }

        public System.Guid SternId { get; set; }
        public Map Stern { get; set; }
    }
}
