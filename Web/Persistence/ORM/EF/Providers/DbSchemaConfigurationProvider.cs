namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Providers
{
    public class DbSchemaConfigurationProvider : Framework.Guama.Core.Persistence.ORM.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade
    {
        public System.Collections.Generic.ICollection<Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.IEntityTypeConfigurator> EntityTypeConfigurators
        {
            get
            {
                return new System.Collections.Generic.List<Framework.Guama.Core.Persistence.ORM.EF.Configurators.Contract.IEntityTypeConfigurator>
                {
                    new Configurators.MapDocumentBridgeEntityTypeConfigurator(),
                    new Configurators.MapImageBridgeEntityTypeConfigurator(),
                    new Configurators.MapKeyWordBridgeEntityTypeConfigurator(),
                    new Configurators.MapVideoBridgeEntityTypeConfigurator(),
                    new Configurators.MapAuthorBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphKeyWordBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphImageBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphVideoBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphDocumentBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphMapBridgeEntityTypeConfigurator(),
                    new Configurators.AuthorEntityTypeConfigurator(),
                    new Configurators.InstitutionEntityTypeConfigurator()
                };
            }
        }

        public string DefaultPersistenceSchema => null;
    }
}
