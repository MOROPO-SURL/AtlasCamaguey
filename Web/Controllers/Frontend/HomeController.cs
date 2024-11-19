using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository;

        public HomeController(IEntityRepository<RichTextBlock, Camaguey.Persistence.Enumerations.RichTextBlockKind> richTextBlockRepository, IEntityRepository<Section, System.Guid> sectionRepository)
        {
            this.sectionRepository = sectionRepository;
            this.richTextBlockRepository = richTextBlockRepository;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.HomeIndexViewModel> Index()
        {
            return View(new Models.View.Frontend.HomeIndexViewModel
            {
                LogotypeContext = richTextBlockRepository.Get(Camaguey.Persistence.Enumerations.RichTextBlockKind.LogotypeContext).Content,
                Sections = sectionRepository.GetAll().OrderBy(entity => entity.Priority).ToArray().Select(entity =>
                {
                    var model = new Models.View.SectionDisplayViewModel();

                    model.Import(entity);

                    return model;
                }).ToArray()
            });
        }
    }
}
