using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapImageBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<MapImageBridge, Map, string, Framework.Guama.Media.Persistence.Entities.Image, System.Guid>
    {
        public MapImageBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.ImageBridges;
        }
    }
}
