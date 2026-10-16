using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Components;

public class MapImageMetadataEntriesViewComponent : ViewComponent
{
    private readonly Framework.Guama.Core.Persistence.Repositories.IQueryEntityRepository<Camaguey.Persistence.Entities.MapImage, System.Guid> mapImageRepository;

    public MapImageMetadataEntriesViewComponent(Framework.Guama.Core.Persistence.Repositories.IQueryEntityRepository<Camaguey.Persistence.Entities.MapImage, System.Guid> mapImageRepository)
    {
        this.mapImageRepository = mapImageRepository;
    }

    public IViewComponentResult Invoke(string mapId, System.Guid imageId)
    {
        return View(mapImageRepository.FindOneBy(entity => entity.MapId == mapId && entity.ImageId == imageId).MetadataEntries.ToArray().Select(entity => new Models.View.CommonMetadataEntryDisplayModel { Id = entity.Id, Name = entity.Name, Value = entity.Value }).ToArray());
    }
}