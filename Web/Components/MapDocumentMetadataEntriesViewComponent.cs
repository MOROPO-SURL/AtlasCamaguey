using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Components
{
    public class MapDocumentMetadataEntriesViewComponent : ViewComponent
    {
        private readonly Framework.Guama.Core.Persistence.Repositories.IEntityRepository<Camaguey.Persistence.Entities.MapDocument, System.Guid> mapDocumentRepository;

        public MapDocumentMetadataEntriesViewComponent(Framework.Guama.Core.Persistence.Repositories.IEntityRepository<Camaguey.Persistence.Entities.MapDocument, System.Guid> mapDocumentRepository)
        {
            this.mapDocumentRepository = mapDocumentRepository;
        }

        public IViewComponentResult Invoke(string mapId, System.Guid documentId)
        {
            return View(mapDocumentRepository.FindOneBy(entity => entity.MapId == mapId && entity.DocumentId == documentId).MetadataEntries.ToArray().Select(entity => new Models.View.CommonMetadataEntryDisplayModel { Id = entity.Id, Name = entity.Name, Value = entity.Value }).ToArray());
        }
    }
}
