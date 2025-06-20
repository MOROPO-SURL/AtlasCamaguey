using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Presentation.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input
{
    public class MapInputViewModel : Framework.Guama.Core.Persistence.Presentation.Models.Contract.IEntityInputModel<Map, string>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Scale")]
        public long Scale { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Priority")]
        public uint Priority { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 3145728, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public bool ResetImage { get; set; }

        public System.Guid? ImageId { get; set; }
        public string ImageExtension { get; set; }

        [FormFileContentTypes(ContentTypes = ["text/csv"], ErrorMessage = "CSV_FileAllowedOnlyMessage")]
        [Display(Name = "MetadataFile")]
        public Microsoft.AspNetCore.Http.IFormFile MetadataFile { get; set; }

        public CommonMetadataEntryDisplayModel[] MetadataEntries { get; set; }

        [Display(Name = "PreLayersJSBlock")]
        public string PreLayersJSBlock { get; set; }

        [Display(Name = "PostLayersJSBlock")]
        public string PostLayersJSBlock { get; set; }

        public Map Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Map entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Scale = entity.Scale;
            Description = entity.Description;
            Priority = (uint)entity.Priority;
            ImageId = entity.ImageId;
            ImageExtension = entity.Image == null ? null : MimeTypes.MimeTypeMap.GetExtension(entity.Image.ContentType);
            MetadataEntries = entity.MetadataEntries.Select(_entity => new CommonMetadataEntryDisplayModel { Id = _entity.Id, Name = _entity.Name, Value = _entity.Value }).ToArray();
            PreLayersJSBlock = entity.PreLayersJSBlock;
            PostLayersJSBlock = entity.PostLayersJSBlock;
        }

        public void Mergue(Map entity)
        {
            entity.Name = Name;
            entity.Scale = Scale;
            entity.Description = Description;
            entity.Priority = (int)Priority;
            entity.PreLayersJSBlock = PreLayersJSBlock;
            entity.PostLayersJSBlock = PostLayersJSBlock;
        }
    }
}

