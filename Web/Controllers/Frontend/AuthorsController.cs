using Microsoft.EntityFrameworkCore;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Frontend
{
    public class AuthorsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly DbContext dbContext;

        public AuthorsController(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Microsoft.AspNetCore.Mvc.ActionResult<Models.View.RichTextBlockDisplayViewModel> Index()
        {
            var entity = dbContext.Set<RichTextBlock>().First(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAuthorsIntro);

            var query = dbContext.Set<MapAuthorBridge>()
                .Include(bridge => bridge.Prow)
                .Include(bridge => bridge.Stern).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(bridge => bridge.Stern).ThenInclude(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .GroupBy(entity => entity.Stern);

            return View(new Models.View.Frontend.AuthorsIndexViewModel
            {
                Intro = entity.Content,
                AuthorOverviews = query.Select(group => BuildAuthorOverviewViewModel(group)).OrderBy(model => model.Author.Name).ToArray()
            });
        }

        private Models.View.Frontend.AuthorsIndexViewModel.AuthorOverviewViewModel BuildAuthorOverviewViewModel(IGrouping<Author, MapAuthorBridge> group)
        {
            var model = new Models.View.Frontend.AuthorsIndexViewModel.AuthorOverviewViewModel
            {
                Author = new Models.View.AuthorDisplayViewModel()
            };

            model.Author.Import(group.Key);

            model.Maps = group.Select(bridge =>
            {
                var _model = new Models.View.MapDisplayViewModel();

                _model.Import(bridge.Prow);

                return _model;
            }).ToArray();

            return model;
        }
    }
}
