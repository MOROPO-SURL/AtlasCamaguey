using Microsoft.AspNetCore.Mvc;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Components;

public class FooterViewComponent : ViewComponent
{
    private readonly IQueryEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository;

    public FooterViewComponent(IQueryEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository)
    {
        this.richTextBlockRepository = richTextBlockRepository;
    }

    public IViewComponentResult Invoke()
    {
        return View(richTextBlockRepository.FindBy(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterLeft || entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterCenter || entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterRight).Select(entity => new Models.View.RichTextBlockDisplayViewModel
        {
            Id = entity.Id,
            Content = entity.Content
        }).ToArray());
    }
}