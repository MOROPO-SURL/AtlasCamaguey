using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Map, string>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Scale")]
        public long Scale { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public Map Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Scale = entity.Scale;
            Description = entity.Description;
        }

        public void Mergue(Map entity)
        {
            entity.Name = Name;
            entity.Scale = Scale;
            entity.Description = Description;
        }
    }
}

