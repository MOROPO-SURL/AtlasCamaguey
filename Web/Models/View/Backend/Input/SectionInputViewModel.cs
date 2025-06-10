using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class SectionInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Section, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Priority")]
        public uint Priority { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public bool ResetImage { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }

        public Section Export()
        {
            var entity = new Section();

            Mergue(entity);

            return entity;
        }

        public void Import(Section entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Priority = (uint)entity.Priority;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
        }

        public void Mergue(Section entity)
        {
            entity.Name = Name;
            entity.Description = Description;
            entity.Priority = (int)Priority;
        }
    }
}

