using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend
{
    public class MapUploadViewModel
    {
        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [FormFileContentTypes(ContentTypes = new string[] { "application/x-zip-compressed" }, ErrorMessage = "Formato de archivo no válido. Se espera un archivo compactado en formato ZIP.")]
        [FormFileMaxLength(Length = 52428800, ErrorMessage = "MapFileLimitSizeExcededMessage")]
        [Display(Name = "MapArchive")]
        public Microsoft.AspNetCore.Http.IFormFile MapArchive { get; set; }
    }
}
