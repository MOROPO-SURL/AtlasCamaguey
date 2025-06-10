using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View;
using MOROPO.CIMAC.Atlas.Camaguey.Web.Models.View.Backend.Input;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Presentation.Models.View;
using MOROPO.Framework.Guama.Core.Presentation.Web.Security.Attributes;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Controllers.Backend.Atlas
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [RequirePermissions(Framework.Guama.Core.Security.Presentation.Constants.Permissions.ShellManager)]
    [Area("atlas")]
    public class ThemesController : Framework.Guama.Core.Presentation.Web.Backend.Controllers.Contract.AbstractEntityManagementController<Theme, System.Guid, ThemeInputViewModel, ThemeDisplayViewModel>
    {
        private readonly IEntityRepository<Epigraph, System.Guid> epigraphRepository;
        private readonly IEntityRepository<Map, string> mapRepository;

        public ThemesController(IEntityRepository<Map, string> mapRepository, IEntityRepository<Epigraph, System.Guid> epigraphRepository, IEntityRepository<Theme, System.Guid> repository, IStringLocalizer<ThemesController> localizer, ILogger<ThemesController> logger) : base(repository, localizer, logger)
        {
            this.epigraphRepository = epigraphRepository;
            this.mapRepository = mapRepository;
        }

        [RequirePermissions(Constants.Permission.ThemeViewer)]
        public IActionResult Index()
        {
            return View();
        }

        protected override System.Func<Theme, object> GetOrderSelector()
        {
            return entity => entity.Name;
        }

        [RequirePermissions(Constants.Permission.ThemeCreator)]
        public override IActionResult Create()
        {
            return base.Create();
        }

        [RequirePermissions(Constants.Permission.ThemeCreator)]
        public override IActionResult Create(ThemeInputViewModel model)
        {
            return base.Create(model);
        }

        [RequirePermissions(Constants.Permission.ThemeEditor)]
        public override IActionResult Edit(System.Guid key)
        {
            return base.Edit(key);
        }

        [RequirePermissions(Constants.Permission.ThemeEditor)]
        public override IActionResult Edit(ThemeInputViewModel model)
        {
            return base.Edit(model);
        }

        [RequirePermissions(Constants.Permission.ThemeRemover)]
        public override IActionResult Delete(System.Guid key)
        {
            return base.Delete(key);
        }

        [RequirePermissions(Constants.Permission.ThemeRemover)]
        public override IActionResult Delete(ThemeDisplayViewModel displayModel)
        {
            return base.Delete(displayModel);
        }

        protected override void PreCreate(Theme entity, ThemeInputViewModel model)
        {
            base.PreCreate(entity, model);

            entity.EpigraphId = model.EpigraphId.Value;
        }

        protected override void PreEdit(Theme entity, ThemeInputViewModel model)
        {
            base.PreEdit(entity, model);

            if (entity.EpigraphId != model.EpigraphId)
            {
                entity.Epigraph = null;
                entity.EpigraphId = model.EpigraphId.Value;
            }
        }

        protected override void ConfigureCreate(ThemeInputViewModel model)
        {
            base.ConfigureCreate(model);

            Populate(model);
        }

        protected override void ConfigureEdit(Theme entity, ThemeInputViewModel model)
        {
            base.ConfigureEdit(entity, model);

            Populate(model);
        }

        protected override void ConfigurePostFailCreateValidation(ThemeInputViewModel model)
        {
            base.ConfigurePostFailCreateValidation(model);

            Populate(model);
        }

        protected override void ConfigurePostFailEditValidation(Theme entity, ThemeInputViewModel model)
        {
            base.ConfigurePostFailEditValidation(entity, model);

            Populate(model);
        }

        private void Populate(ThemeInputViewModel model)
        {
            model.EpigraphSelectListItems = epigraphRepository.GetAll().OrderBy(entity => entity.Name).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = entity.Id.ToString(),
                Text = entity.Name
            }).ToArray();
        }

        #region Maps

        [HttpGet]
        [Route("edit/{key}/maps")]
        [RequirePermissions(Constants.Permission.ThemeEditor)]
        public ActionResult EditMaps(System.Guid key)
        {
            var entity = Repository.Get(key);

            var releatedMaps = entity.MapBridges.Select(bridge => bridge.Stern).ToArray();

            var model = new Models.View.Backend.Management.ThemeMapsManagementViewModel
            {
                Theme = new ThemeDisplayViewModel(),
                Maps = entity.MapBridges.Select(bridge =>
                {
                    var result = new MapDisplayViewModel();

                    result.Import(bridge.Stern);

                    return result;
                }).ToArray(),
                AvailableMapSelectListItems = mapRepository.GetAll().ToArray().Except(releatedMaps).Select(entity => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = entity.Id.ToString(),
                    Text = entity.Name
                }).ToArray()
            };

            model.Theme.Import(entity);

            return View(model);
        }

        [HttpPost]
        [Route("edit/{key}/maps/add")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.ThemeEditor)]
        public IActionResult AddMap(System.Guid key, string mapId)
        {
            try
            {
                var entity = Repository.Get(key);

                if (mapId != default)
                {
                    try
                    {
                        entity.MapBridges.Add(new ThemeMapBridge
                        {
                            ProwId = entity.Id,
                            SternId = mapId
                        });

                        Repository.Update(entity, User.Identity.Name);

                        SetDefaultNotification(NotificationType.Success, GetCreationSuccessMessage());

                        return RedirectToAction("editmaps", new { key = entity.Id });
                    }
                    catch (Framework.Guama.Core.Presentation.Web.Frontend.InvalidModelException e)
                    {
                        ModelState.AddModelError(e.ModelKey, e.ErrorMessage);
                    }
                }

                SetDefaultNotification(NotificationType.Danger, GetInvalidMessage());

                return RedirectToAction("editmaps", new { key = entity.Id });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetDefaultActionResult();
        }

        [HttpPost]
        [Route("edit/{key}/maps/delete")]
        [ValidateAntiForgeryToken]
        [RequirePermissions(Constants.Permission.ThemeEditor)]
        public IActionResult DeleteMap(System.Guid key, string mapId)
        {
            try
            {
                var entity = Repository.Get(key);

                var bridge = entity.MapBridges.First(b => b.SternId == mapId);

                entity.MapBridges.Remove(bridge);

                Repository.Update(entity, User.Identity.Name);

                SetDefaultNotification(NotificationType.Success, GetDeletionSuccessMessage());

                return RedirectToAction("editmaps", new { key = entity.Id });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                logger.LogError(e.InnerException, e.Message);

                SetDefaultNotification(NotificationType.Warning, GetOnDeletionDependecyConstrainErrorMessage());

                return RedirectToAction("editmaps", new { key });
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);
            }

            SetDefaultNotification(NotificationType.Danger, GetNotFoundMessage());

            return GetSuccessDeleteActionResult();
        }

        #endregion
    }
}

