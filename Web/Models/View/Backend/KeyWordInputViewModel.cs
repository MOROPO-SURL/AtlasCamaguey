using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class KeyWordInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<KeyWord, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public KeyWord Export()
        {
            var entity = new KeyWord();

            Mergue(entity);

            return entity;
        }

        public void Import(KeyWord entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }

        public void Mergue(KeyWord entity)
        {
            entity.Name = Name;
        }
    }
}

