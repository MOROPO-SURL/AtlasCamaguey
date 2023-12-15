using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class MapDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<Map, string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long Scale { get; set; }

        public string Description { get; set; }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Scale = entity.Scale;
            Description = entity.Description;
        }
    }
}

