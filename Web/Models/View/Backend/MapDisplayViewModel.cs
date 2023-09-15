using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Map, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Project { get; set; }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Project = entity.Project;
        }
    }
}

