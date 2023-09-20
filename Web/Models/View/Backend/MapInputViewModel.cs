using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Map, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "QgisProjectPath")]
        public string QgisProjectPath { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public Map Export()
        {
            var entity = new Map();

            Mergue(entity);

            return entity;
        }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            QgisProjectPath = entity.QgisProjectPath;
            Description = entity.Description;
        }

        public void Mergue(Map entity)
        {
            entity.Name = Name;
            entity.QgisProjectPath = QgisProjectPath;
            entity.Description = Description;
        }
    }
}

