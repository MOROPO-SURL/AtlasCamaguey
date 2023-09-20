namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapKeyWordBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Map, System.Guid, KeyWord, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Map Prow { get; set; }

        public System.Guid SternId { get; set; }
        public KeyWord Stern { get; set; }
    }
}
