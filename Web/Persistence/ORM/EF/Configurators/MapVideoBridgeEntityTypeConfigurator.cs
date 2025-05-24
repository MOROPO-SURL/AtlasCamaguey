using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapVideoBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<MapVideoBridge, Map, string, Framework.Guama.Media.Persistence.Entities.Video, System.Guid>
    {
        public MapVideoBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.VideoBridges;
        }
    }
}
