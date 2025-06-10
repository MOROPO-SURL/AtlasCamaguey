using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class ThemeInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Theme, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Priority")]
        public uint Priority { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Epigraph")]
        public System.Guid? EpigraphId { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] EpigraphSelectListItems { get; set; }

        public Theme Export()
        {
            var entity = new Theme();

            Mergue(entity);

            return entity;
        }

        public void Import(Theme entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Priority = (uint)entity.Priority;
            EpigraphId = entity.EpigraphId;
        }

        public void Mergue(Theme entity)
        {
            entity.Name = Name;
            entity.Priority = (int)Priority;
        }
    }
}

