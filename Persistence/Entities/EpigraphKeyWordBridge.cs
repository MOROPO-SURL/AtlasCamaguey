namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class EpigraphKeyWordBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Epigraph, System.Guid, KeyWord, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Epigraph Prow { get; set; }

        public System.Guid SternId { get; set; }
        public KeyWord Stern { get; set; }
    }
}
