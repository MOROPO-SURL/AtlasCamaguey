using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class InstitutionInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Institution, System.Guid>
    {
        public System.Guid Id { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }

        public bool ResetImage { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public Institution Export()
        {
            var entity = new Institution();

            Mergue(entity);

            return entity;
        }

        public void Import(Institution entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.ImageId;
            ImageExtension = entity.Identity.ImageId == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Identity.Image.ContentType);
            Name = entity.Identity.Name;
            Alias = entity.Alias;
            Description = entity.Identity.About;
        }

        public void Mergue(Institution entity)
        {
            entity.Identity.Name = Name;
            entity.Alias = Alias;
            entity.Identity.About = Description;
        }
    }
}
