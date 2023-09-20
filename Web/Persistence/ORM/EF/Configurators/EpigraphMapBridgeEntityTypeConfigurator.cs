using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class EpigraphMapBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.AbstractBridgeEntityTypeConfigurator<EpigraphMapBridge, Epigraph, System.Guid, Map, System.Guid>
    {
        public EpigraphMapBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.MapBridges;
        }
    }
}
