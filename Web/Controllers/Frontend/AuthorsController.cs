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

            var universe = dbContext.Set<MapAuthorBridge>()
                .Include(bridge => bridge.Prow)
                .Include(bridge => bridge.Stern).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .Include(bridge => bridge.Stern).ThenInclude(entity => entity.Institution).ThenInclude(entity => entity.Identity).ThenInclude(entity => entity.Image)
                .ToArray();

            return View(new Models.View.Frontend.AuthorsIndexViewModel
            {
                Intro = entity.Content,
                AuthorOverviews = universe.GroupBy(entity => entity.Stern).Select(group => BuildAuthorOverviewViewModel(group)).OrderBy(model => model.Author.Name).ToArray()
            });
        }

        private Models.View.Frontend.AuthorsIndexViewModel.AuthorOverviewViewModel BuildAuthorOverviewViewModel(IGrouping<Author, MapAuthorBridge> group)
        {
            var model = new Models.View.Frontend.AuthorsIndexViewModel.AuthorOverviewViewModel
            {
                Author = new Models.View.AuthorDisplayViewModel()
            };

            model.Author.Import(group.Key);

            model.Maps = new System.Collections.Generic.Dictionary<Models.View.MapDisplayViewModel, Models.View.EpigraphDisplayViewModel[]>();

            foreach (var item in group)
            {
                var themes = dbContext.Set<ThemeMapBridge>().Include(bridge => bridge.Prow).Where(bridge => bridge.Stern.Id == item.Prow.Id).ToArray();

                var _model = new Models.View.MapDisplayViewModel();

                _model.Import(item.Prow);

                model.Maps.Add(_model, themes.Select(bridge => new Models.View.EpigraphDisplayViewModel
                {
                    Id = bridge.Prow.EpigraphId,
                    Name = item.Prow.Name
                }).ToArray());
            }

            return model;
        }
    }
}
