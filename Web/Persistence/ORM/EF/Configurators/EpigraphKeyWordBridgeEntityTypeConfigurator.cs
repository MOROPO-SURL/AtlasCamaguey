using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Configurators
{
    public class EpigraphKeyWordBridgeEntityTypeConfigurator : Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.AbstractBridgeEntityTypeConfigurator<EpigraphKeyWordBridge, Epigraph, System.Guid, KeyWord, System.Guid>
    {
        public EpigraphKeyWordBridgeEntityTypeConfigurator()
        {
            prowNavigationExpression = entity => entity.KeyWordBridges;
        }
    }
}
