namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Providers
{
    public class DbSchemaConfigurationProvider : Framework.Guama.Core.Persistence.ORM.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade
    {
        public System.Collections.Generic.ICollection<Framework.Guama.Core.Persistence.ORM.EF.Configurators.IEntityTypeConfigurator> EntityTypeConfigurators
        {
            get
            {
                return new System.Collections.Generic.List<Framework.Guama.Core.Persistence.ORM.EF.Configurators.IEntityTypeConfigurator>
                {
                    new Configurators.EpigraphKeyWordBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphImageBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphVideoBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphDocumentBridgeEntityTypeConfigurator(),
                    new Configurators.MapEntityTypeConfigurator()
                };
            }
        }

        public string DefaultPersistenceSchema => null;
    }
}
