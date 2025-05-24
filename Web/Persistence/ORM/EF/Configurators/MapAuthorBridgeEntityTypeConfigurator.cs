using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class MapAuthorBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<MapAuthorBridge, Map, string, Author, System.Guid>
    {
        public MapAuthorBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.AuthorBridges;
        }
    }
}
