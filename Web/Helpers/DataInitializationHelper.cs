using System.Linq;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
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

            #region Epigraph

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.EpigraphViewer)))
            {
                permissionDbSet.Add(Constants.Permission.EpigraphViewer.ToPermissionEntity(Constants.PermissionsFamily.EpigraphManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.EpigraphCreator)))
            {
                permissionDbSet.Add(Constants.Permission.EpigraphCreator.ToPermissionEntity(Constants.PermissionsFamily.EpigraphManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.EpigraphEditor)))
            {
                permissionDbSet.Add(Constants.Permission.EpigraphEditor.ToPermissionEntity(Constants.PermissionsFamily.EpigraphManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.EpigraphRemover)))
            {
                permissionDbSet.Add(Constants.Permission.EpigraphRemover.ToPermissionEntity(Constants.PermissionsFamily.EpigraphManagement));
            }
            #endregion

            #region Author

            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.AuthorViewer)))
            {
                permissionDbSet.Add(Constants.Permission.AuthorViewer.ToPermissionEntity(Constants.PermissionsFamily.AuthorManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.AuthorCreator)))
            {
                permissionDbSet.Add(Constants.Permission.AuthorCreator.ToPermissionEntity(Constants.PermissionsFamily.AuthorManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.AuthorEditor)))
            {
                permissionDbSet.Add(Constants.Permission.AuthorEditor.ToPermissionEntity(Constants.PermissionsFamily.AuthorManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.AuthorRemover)))
            {
                permissionDbSet.Add(Constants.Permission.AuthorRemover.ToPermissionEntity(Constants.PermissionsFamily.AuthorManagement));
            }
            #endregion

            #region Institution
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.InstitutionViewer)))
            {
                permissionDbSet.Add(Constants.Permission.InstitutionViewer.ToPermissionEntity(Constants.PermissionsFamily.InstitutionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.InstitutionCreator)))
            {
                permissionDbSet.Add(Constants.Permission.InstitutionCreator.ToPermissionEntity(Constants.PermissionsFamily.InstitutionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.InstitutionEditor)))
            {
                permissionDbSet.Add(Constants.Permission.InstitutionEditor.ToPermissionEntity(Constants.PermissionsFamily.InstitutionManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.InstitutionViewer)))
            {
                permissionDbSet.Add(Constants.Permission.InstitutionViewer.ToPermissionEntity(Constants.PermissionsFamily.InstitutionManagement));
            }
            #endregion

            #region RichTextBlock
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.RichTextBlockViewer)))
            {
                permissionDbSet.Add(Constants.Permission.RichTextBlockViewer.ToPermissionEntity(Constants.PermissionsFamily.RichTextBlokManagement));
            }
            if (!permissionDbSet.Any(role => role.Code.Equals(Constants.Permission.RichTextBlockEditor)))
            {
                permissionDbSet.Add(Constants.Permission.RichTextBlockEditor.ToPermissionEntity(Constants.PermissionsFamily.RichTextBlokManagement));
            }

            var richTextBlockDbSet = dbContext.Set<RichTextBlock>();

            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.LogotypeContext))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.LogotypeContext, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterLeft)) 
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterLeft, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterCenter))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterCenter, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterRight))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.FooterRight, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageHomeIntro)) 
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.PageHomeIntro, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageContactIntro))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.PageContactIntro, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAuthorsIntro))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAuthorsIntro, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAboutContent))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.PageAboutContent, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
            }
            if (!richTextBlockDbSet.Any(entity => entity.Id == Camaguey.Persistence.Enumerations.RichTextBlockKind.PageHelpContent))
            {
                richTextBlockDbSet.Add(new RichTextBlock { Id = Camaguey.Persistence.Enumerations.RichTextBlockKind.PageHelpContent, Content = "Stet elit molestie invidunt tincidunt lobortis erat augue sadipscing nonumy suscipit voluptua duo nonumy et mazim duis et qui et" });
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
