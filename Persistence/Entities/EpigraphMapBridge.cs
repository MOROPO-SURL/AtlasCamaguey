namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class EpigraphMapBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Epigraph, System.Guid, Map, string>
    {
        public System.Guid ProwId { get; set; }
        public Epigraph Prow { get; set; }

        public string SternId { get; set; }
        public Map Stern { get; set; }
    }
}
