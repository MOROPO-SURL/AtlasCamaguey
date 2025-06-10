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
                    Title = "Temáticas",
                    Controller = "themes",
                    Action = "index",
                    Area = "atlas",
                    Role = Constants.Permission.ThemeViewer
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

            var contentSections = new List<ManagementDashboardSectionViewModel>
            {
                new ManagementDashboardSectionViewModel
                {
                    Title = "Bloques de Texto",
                    Controller = "richtextblocks",
                    Action = "index",
                    Area = "content",
                    Role = Constants.Permission.RichTextBlockViewer
                }
            };

            return new Dictionary<string, ICollection<ManagementDashboardSectionViewModel>>
            {
                { "security", securitySections },
                { "atlas", atlasSections },
                { "complements", complementarySections },
                { "content", contentSections }
            };
        }
    }
}
