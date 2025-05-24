using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class EpigraphVideoBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<EpigraphVideoBridge, Epigraph, System.Guid, Framework.Guama.Media.Persistence.Entities.Video, System.Guid>
    {
        public EpigraphVideoBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.VideoBridges;
        }
    }
}
