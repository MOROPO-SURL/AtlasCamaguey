using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class SectionController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEntityRepository<Section, System.Guid> sectionRepository;
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;

        public SectionController(IEntityRepository<Section, System.Guid> sectionRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository)
        {
            this.epigraphRepository = epigraphRepository;
            this.sectionRepository = sectionRepository;
        }

        [Microsoft.AspNetCore.Mvc.Route("{key}")]
        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.Frontend.SectionIndexViewModel> Index(System.Guid key)
        {
            var section = sectionRepository.Get(key);

            var model = new Models.View.Frontend.SectionIndexViewModel
            {
                Section = new Models.View.SectionDisplayViewModel(),
                Epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == section.Id).ToArray().Select(entity =>
                {
                    var _model = new Models.View.EpigraphDisplayViewModel();

                    _model.Import(entity);

                    return _model;
                }).ToArray()
            };

            model.Section.Import(section);

            return View(model);
        }
    }
}
