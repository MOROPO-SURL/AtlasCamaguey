using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    public class KeyWordDisplayViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityDisplayModel<KeyWord, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public void Import(KeyWord entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}

