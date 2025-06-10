using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class ThemeMapBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<ThemeMapBridge, Theme, System.Guid, Map, string>
    {
        public ThemeMapBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.MapBridges;
        }
    }
}
