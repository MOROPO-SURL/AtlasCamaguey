using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class HomeViewModel
    {
        public Dictionary<string, ICollection<Framework.Guama.Core.Presentation.Web.Backend.Models.View.ManagementDashboardSectionViewModel>> Sections { get; set; }

        public int UserTotalCount { get; set; }
        public int UserDisabledCount { get; set; }
        public int TraceTotalCount { get; set; }
        public int TraceWeekCount { get; set; }
        public int SectionCount { get; set; }
        public int EpigraphCount { get; set; }
        public int MapCount { get; set; }

        public Dictionary<string, long> UserActivityMap { get; set; }

        public Dictionary<string, int> WeekTracesMap { get; set; }
    }
}
