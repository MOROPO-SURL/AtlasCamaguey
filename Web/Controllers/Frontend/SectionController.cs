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
                Current = new Models.View.SectionDisplayViewModel(),
                Sections = sectionRepository.GetAll().ToArray().Select(entity =>
                {
                    var _model = new Models.View.SectionDisplayViewModel();

                    _model.Import(entity);

                    _model.Epigraphs = epigraphRepository.FindBy(_entity => _entity.SectionId == entity.Id).ToArray().Select(_entity =>
                    {
                        var __model = new Models.View.EpigraphDisplayViewModel();

                        __model.Import(_entity);

                        return __model;
                    }).ToArray();

                    return _model;
                }).ToArray()
            };

            model.Current.Import(section);
            model.Current.Epigraphs = epigraphRepository.FindBy(entity => entity.SectionId == section.Id).ToArray().Select(entity =>
            {
                var _model = new Models.View.EpigraphDisplayViewModel();

                _model.Import(entity);

                return _model;
            }).ToArray();

            return View(model);
        }
    }
}
