using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Media.Persistence.Entities;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Managers.Facade;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DownloadController : Framework.Guama.Core.Presentation.Web.Common.Controllers.BaseWebController
    {
        private readonly IEntityRepository<Document, System.Guid> documentRepository;
        private readonly IDocumentFileManagerFacade documentFileManager;

        public DownloadController(IDocumentFileManagerFacade documentFileManager, IEntityRepository<Document, System.Guid> documentRepository, Microsoft.Extensions.Localization.IStringLocalizer<DownloadController> localizer, ILogger<DownloadController> logger) : base(localizer, logger)
        {
            this.documentRepository = documentRepository;
            this.documentFileManager = documentFileManager;
        }

        public IActionResult Document(System.Guid key)
        {
            var entity = documentRepository.Get(key);

            return new FileContentResult(documentFileManager.GetDocumentFileContent(entity), entity.ContentType)
            {
                FileDownloadName = string.Format("{0}.{1}", entity.Name, MimeTypes.MimeTypeMap.GetExtension(entity.ContentType))
            };
        }
    }
}
