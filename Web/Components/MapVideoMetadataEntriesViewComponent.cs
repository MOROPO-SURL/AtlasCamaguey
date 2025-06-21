using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Components
{
    public class MapVideoMetadataEntriesViewComponent : ViewComponent
    {
        private readonly Framework.Guama.Core.Persistence.Repositories.IEntityRepository<Camaguey.Persistence.Entities.MapVideo, System.Guid> mapVideoRepository;

        public MapVideoMetadataEntriesViewComponent(Framework.Guama.Core.Persistence.Repositories.IEntityRepository<Camaguey.Persistence.Entities.MapVideo, System.Guid> mapVideoRepository)
        {
            this.mapVideoRepository = mapVideoRepository;
        }

        public IViewComponentResult Invoke(string mapId, System.Guid videoId)
        {
            return View(mapVideoRepository.FindOneBy(entity => entity.MapId == mapId && entity.VideoId == videoId).MetadataEntries.ToArray().Select(entity => new Models.View.CommonMetadataEntryDisplayModel { Id = entity.Id, Name = entity.Name, Value = entity.Value }).ToArray());
        }
    }
}
