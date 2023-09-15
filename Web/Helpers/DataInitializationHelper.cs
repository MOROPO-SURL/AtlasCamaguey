using System.Linq;
using MOROPO.Framework.Guama.Core.Security.Common.Persistence.ORM.EF.Extensions;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Helpers
{
    public class DataInitializationHelper : Framework.Guama.Core.Persistence.ORM.EF.Helpers.Facade.IDataInitializationHelper
    {
        public void InitialiseData(Microsoft.EntityFrameworkCore.DbContext dbContext)
        {
            var permissionDbSet = dbContext.Set<Framework.Guama.Core.Security.Common.Persistence.Entities.Permission>();

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.TraceViewer)))
            {
                permissionDbSet.Add(Constants.Permission.TraceViewer.ToPermissionEntity());
            }

            #region KeyWord

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.KeyWordViewer)))
            {
                permissionDbSet.Add(Constants.Permission.KeyWordViewer.ToPermissionEntity(Constants.PermissionsFamily.KeyWordManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.KeyWordCreator)))
            {
                permissionDbSet.Add(Constants.Permission.KeyWordCreator.ToPermissionEntity(Constants.PermissionsFamily.KeyWordManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.KeyWordEditor)))
            {
                permissionDbSet.Add(Constants.Permission.KeyWordEditor.ToPermissionEntity(Constants.PermissionsFamily.KeyWordManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.KeyWordRemover)))
            {
                permissionDbSet.Add(Constants.Permission.KeyWordRemover.ToPermissionEntity(Constants.PermissionsFamily.KeyWordManagement));
            }
            #endregion

            #region Map

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.MapViewer)))
            {
                permissionDbSet.Add(Constants.Permission.MapViewer.ToPermissionEntity(Constants.PermissionsFamily.MapManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.MapCreator)))
            {
                permissionDbSet.Add(Constants.Permission.MapCreator.ToPermissionEntity(Constants.PermissionsFamily.MapManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.MapEditor)))
            {
                permissionDbSet.Add(Constants.Permission.MapEditor.ToPermissionEntity(Constants.PermissionsFamily.MapManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.MapRemover)))
            {
                permissionDbSet.Add(Constants.Permission.MapRemover.ToPermissionEntity(Constants.PermissionsFamily.MapManagement));
            }
            #endregion

            #region Section

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.SectionViewer)))
            {
                permissionDbSet.Add(Constants.Permission.SectionViewer.ToPermissionEntity(Constants.PermissionsFamily.SectionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.SectionCreator)))
            {
                permissionDbSet.Add(Constants.Permission.SectionCreator.ToPermissionEntity(Constants.PermissionsFamily.SectionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.SectionEditor)))
            {
                permissionDbSet.Add(Constants.Permission.SectionEditor.ToPermissionEntity(Constants.PermissionsFamily.SectionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.SectionRemover)))
            {
                permissionDbSet.Add(Constants.Permission.SectionRemover.ToPermissionEntity(Constants.PermissionsFamily.SectionManagement));
            }
            #endregion


            if (dbContext is Framework.Guama.Core.Persistence.ORM.EF.Context.Contract.ITraceableDbContext)
            {
                (dbContext as Framework.Guama.Core.Persistence.ORM.EF.Context.Contract.ITraceableDbContext).SaveChanges("system");
            }
            else
            {
                dbContext.SaveChanges();
            }
        }
    }
}
