namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class MapAuthorBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Map, System.Guid, Author, System.Guid>
    {
        public System.Guid ProwId { get; set; }
        public Map Prow { get; set; }

        public System.Guid SternId { get; set; }
        public Author Stern { get; set; }
    }
}
