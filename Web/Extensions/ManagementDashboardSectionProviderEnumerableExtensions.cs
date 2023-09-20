using MOROPO.Framework.Guama.Core.Presentation.Web.Backend.Models.View;
using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Extensions
{
    public static class ManagementDashboardSectionProviderEnumerableExtensions
    {
        public static Dictionary<string, ICollection<ManagementDashboardSectionViewModel>> ToManagementDashboardSectionGroups(this IEnumerable<Framework.Guama.Core.Presentation.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> self)
        {
            var securitySections = new List<ManagementDashboardSectionViewModel>();

            foreach (var provider in self)
            {
                if (provider is Framework.Guama.Security.Personal.Presentation.Web.Backend.Providers.ManagementDashboardSectionProvider)
                {
                    securitySections.AddRange(provider.GetManagementDashboardSections());
                }
            }

            securitySections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Registros",
                Controller = "traces",
                Action = "index",
                Area = "security",
                Role = Constants.Permission.TraceViewer
            });

            var atlasSections = new List<ManagementDashboardSectionViewModel>
            {
                new ManagementDashboardSectionViewModel
                {
                    Title = "Secciones",
                    Controller = "sections",
                    Action = "index",
                    Area = "atlas",
                    Role = Constants.Permission.SectionViewer
                },
                new ManagementDashboardSectionViewModel
                {
                    Title = "Epígrafes",
                    Controller = "epigraphs",
                    Action = "index",
                    Area = "atlas",
                    Role = Constants.Permission.EpigraphViewer
                },
                new ManagementDashboardSectionViewModel
                {
                    Title = "Mapas",
                    Controller = "maps",
                    Action = "index",
                    Area = "atlas",
                    Role = Constants.Permission.MapViewer
                },
                new ManagementDashboardSectionViewModel
                {
                    Title = "Autores",
                    Controller = "authors",
                    Action = "index",
                    Area = "atlas",
                    Role = Constants.Permission.AuthorViewer
                }
            };

            var complementarySections = new List<ManagementDashboardSectionViewModel>
            {
                new ManagementDashboardSectionViewModel
                {
                    Title = "Instituciones",
                    Controller = "institutions",
                    Action = "index",
                    Area = "complements",
                    Role = Constants.Permission.InstitutionViewer
                },
                new ManagementDashboardSectionViewModel
                {
                    Title = "Palabras Claves",
                    Controller = "keywords",
                    Action = "index",
                    Area = "complements",
                    Role = Constants.Permission.KeyWordViewer
                }
            };

            return new Dictionary<string, ICollection<ManagementDashboardSectionViewModel>>
            {
                { "Seguridad", securitySections },
                { "Contenido del Atlas", atlasSections },
                { "Complementarios", complementarySections }
            };
        }
    }
}
