using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Map, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string QgisProjectPath { get; set; }

        public string Description { get; set; }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            QgisProjectPath = entity.QgisProjectPath;
            Description = entity.Description;
        }
    }
}

