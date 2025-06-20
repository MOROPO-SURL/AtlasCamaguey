using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class DocumentInputViewModel
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [FormFileContentTypes(ContentTypes = ["application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "image/jpeg", "image/png", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/x-zip-compressed", "application/vnd.android.package-archive", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation"], ErrorMessage = "DOCs_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 26214400, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "File")]
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [FormFileContentTypes(ContentTypes = ["text/csv"], ErrorMessage = "CSV_FileAllowedOnlyMessage")]
        [Display(Name = "MatadataFile")]
        public Microsoft.AspNetCore.Http.IFormFile MatadataFile { get; set; }
    }
}
