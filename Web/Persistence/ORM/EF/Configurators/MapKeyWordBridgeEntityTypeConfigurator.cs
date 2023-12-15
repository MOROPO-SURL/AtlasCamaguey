using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapKeyWordBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.AbstractBridgeEntityTypeConfigurator<MapKeyWordBridge, Map, string, KeyWord, System.Guid>
    {
        public MapKeyWordBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.KeyWordBridges;
        }
    }
}
