using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class ImageInputViewModel
    {
        public System.Guid Id { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "File")]
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [FormFileContentTypes(ContentTypes = ["text/csv"], ErrorMessage = "CSV_FileAllowedOnlyMessage")]
        [Display(Name = "MatadataFile")]
        public Microsoft.AspNetCore.Http.IFormFile MatadataFile { get; set; }
    }
}
