using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapVideoBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.AbstractBridgeEntityTypeConfigurator<MapVideoBridge, Map, System.Guid, Framework.Guama.Media.Persistence.Entities.Video, System.Guid>
    {
        public MapVideoBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.VideoBridges;
        }
    }
}
