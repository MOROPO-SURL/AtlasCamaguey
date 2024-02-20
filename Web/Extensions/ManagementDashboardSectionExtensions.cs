namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Extensions
{
    public static class ManagementDashboardSectionExtensions
    {
        public static Microsoft.AspNetCore.Html.HtmlString ToIconographyHtmlString(this Framework.Guama.Core.Presentation.Web.Backend.Models.View.ManagementDashboardSectionViewModel target)
        {
            if (target.Controller == "users") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-account\"></span>");

            if (target.Controller == "roles") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-account-group\"></span>");

            if (target.Controller == "traces") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-timeline-clock\"></span>");

            if (target.Controller == "authors") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-account-tie\"></span>");

            if (target.Controller == "institutions") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-domain\"></span>");

            if (target.Controller == "sections") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-bookmark\"></span>");

            if (target.Controller == "epigraphs") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-label\"></span>");

            if (target.Controller == "maps") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-earth-box\"></span>");

            if (target.Controller == "keywords") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-tag-text\"></span>");

            if (target.Controller == "richtextblocks") return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-text\"></span>");

            return new Microsoft.AspNetCore.Html.HtmlString("<span class=\"mdi mdi-toy-brick-outline\"></span>");
        }
    }
}
