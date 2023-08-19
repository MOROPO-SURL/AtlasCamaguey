using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View
{
    [Microsoft.AspNetCore.Mvc.ModelMetadataType(typeof(Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel))]
    public class UserProfileViewModel : Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel
    {
        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Display(Name = "Sex")]
        public Framework.Guama.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 1048576, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public bool ResetImage { get; set; }

        public System.Guid? ImageId { get; set; }

        public string ImageExtension { get; set; }
    }
}
