namespace MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities
{
    public class ThemeMapBridge : Framework.Guama.Core.Persistence.Entities.Contract.IBridge<Theme, System.Guid, Map, string>
    {
        public System.Guid ProwId { get; set; }
        public Theme Prow { get; set; }

        public string SternId { get; set; }
        public Map Stern { get; set; }
    }
}
