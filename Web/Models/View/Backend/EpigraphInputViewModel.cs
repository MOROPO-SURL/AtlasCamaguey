using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class EpigraphInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Epigraph, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public bool ResetImage { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Section")]
        public System.Guid? SectionId { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] SectionSelectListItems { get; set; }

        public Epigraph Export()
        {
            var entity = new Epigraph();

            Mergue(entity);

            return entity;
        }

        public void Import(Epigraph entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
            SectionId = entity.SectionId;
        }

        public void Mergue(Epigraph entity)
        {
            entity.Name = Name;
            entity.Description = Description;
        }
    }
}

