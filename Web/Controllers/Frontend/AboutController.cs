using MOROPO.Framework.Guama.Core.Persistence.Repositories;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    public class AboutController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository;

        public AboutController(IEntityRepository<Camaguey.Persistence.Entities.RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository)
        {
            this.richTextBlockRepository = richTextBlockRepository;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.RichTextBlockDisplayViewModel> Index()
        {
            var entity = richTextBlockRepository.Get(Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAboutContent);

            return View(new Models.View.RichTextBlockDisplayViewModel
            {
                Id = entity.Id,
                Content = entity.Content
            });
        }
    }
}
