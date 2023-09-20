using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class AuthorInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Author, System.Guid>
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

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Description")]
        public string About { get; set; }

        [Display(Name = "NamePrefix")]
        public string NamePrefix { get; set; }

        [Display(Name = "Institution")]
        public System.Guid? InstitutionId { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] InstitutionSelectListItems { get; set; }

        public Author Export()
        {
            var entity = new Author();

            Mergue(entity);

            return entity;
        }

        public void Import(Author entity)
        {
            Id = entity.Id;
            ImageId = entity.Identity.ImageId;
            ImageExtension = entity.Identity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Identity.Image.ContentType);
            Name = entity.Identity.Name;
            LastName = entity.Identity.LastName;
            About = entity.Identity.About;
            NamePrefix = entity.NamePrefix;
            InstitutionId = entity.InstitutionId;
        }

        public void Mergue(Author entity)
        {
            entity.Identity.Name = Name;
            entity.Identity.LastName = LastName;
            entity.Identity.About = About;
            entity.NamePrefix = NamePrefix;
        }
    }
}
