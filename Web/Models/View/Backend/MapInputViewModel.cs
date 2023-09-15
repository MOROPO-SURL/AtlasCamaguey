using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Map, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Project { get; set; }

        public Map Export()
        {
            var entity = new Map();

            Mergue(entity);

            return entity;
        }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Project = entity.Project;
        }

        public void Mergue(Map entity)
        {
            entity.Project = Project;
        }
    }
}

