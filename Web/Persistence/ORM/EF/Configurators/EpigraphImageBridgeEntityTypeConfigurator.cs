using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class EpigraphImageBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<EpigraphImageBridge, Epigraph, System.Guid, Framework.Guama.Media.Persistence.Entities.Image, System.Guid>
    {
        public EpigraphImageBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.ImageBridges;
        }
    }
}
