using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
	public class NavbarViewModel
    {
        public NavbarViewModel()
        {
            UsersList = new List<string>();
            TracesbyUsers = new List<long>();
            WeekStatisticsColumns = new List<string>();
            WeekTracesStatisticsValues = new List<int>();
        }

        public Dictionary<string, ICollection<Framework.Guama.Core.Presentation.Web.Backend.Models.View.ManagementDashboardSectionViewModel>> Sections { get; set; }

        public List<string> UsersList { get; set; }
        public List<long> TracesbyUsers { get; set; }
        public List<string> WeekStatisticsColumns { get; set; }
        public List<int> WeekTracesStatisticsValues { get; set; }
    }
}

