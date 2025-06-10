using System.Collections.Generic;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Frontend
{
    public class ThematicStructureIndexViewModel
    {
        public string Intro { get; set; }

        public Dictionary<SectionDisplayViewModel, Dictionary<EpigraphDisplayViewModel, Dictionary<ThemeDisplayViewModel, MapDisplayViewModel[]>>> Structure { get; set; }
    }
}
