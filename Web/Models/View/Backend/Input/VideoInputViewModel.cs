using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class VideoInputViewModel
    {
        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [FormFileContentTypes(ContentTypes = ["video/mp4"], ErrorMessage = "MP4_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 47185920, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "File")]
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [FormFileContentTypes(ContentTypes = ["text/csv"], ErrorMessage = "CSV_FileAllowedOnlyMessage")]
        [Display(Name = "MatadataFile")]
        public Microsoft.AspNetCore.Http.IFormFile MatadataFile { get; set; }
    }
}
