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
                    new Configurators.MapKeyWordBridgeEntityTypeConfigurator(),
                    new Configurators.MapAuthorBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphKeyWordBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphImageBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphVideoBridgeEntityTypeConfigurator(),
                    new Configurators.EpigraphDocumentBridgeEntityTypeConfigurator(),
                    new Configurators.ThemeMapBridgeEntityTypeConfigurator(),
                    new Configurators.AuthorEntityTypeConfigurator(),
                    new Configurators.InstitutionEntityTypeConfigurator(),
                    new Configurators.MapImageEntityTypeConfigurator(),
                    new Configurators.MapVideoEntityTypeConfigurator(),
                    new Configurators.MapDocumentEntityTypeConfigurator(),
                    new Configurators.MapMetadataEntryEntityTypeConfigurator(),
                    new Configurators.MapImageMetadataEntryEntityTypeConfigurator(),
                    new Configurators.MapVideoMetadataEntryEntityTypeConfigurator(),
                    new Configurators.MapDocumentMetadataEntryEntityTypeConfigurator()
                };
            }
        }

        public string DefaultPersistenceSchema => null;
    }
}
