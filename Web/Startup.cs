using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MOROPO.CIMAC.Atlas.Camaguey.Persistence.Entities;
using MOROPO.Framework.Guama.Core.Persistence.Repositories;
using MOROPO.Framework.Guama.Core.Security.Common.Persistence.Extensions;
using MOROPO.Framework.Guama.Core.Security.Common.Presentation.Web.Frontend.Extensions;
using MOROPO.Framework.Guama.Media.Presentation.Web.Common.Extensions;
using MOROPO.Framework.Guama.Security.Personal.Presentation.Web.Backend.Extensions;
using Serilog;
using System.Linq;

namespace MOROPO.CIMAC.Atlas.Camaguey.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            this.configuration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Framework.Guama.Media.Presentation.Web.Common.Options.RelativeMediaFileManagementOptions>(configuration.GetSection("FileManagement"));

            services.AddDbContext<Persistence.ORM.EF.ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DbContext, Persistence.ORM.EF.ApplicationDbContext>();

            services.PerformCoreWebSecurityCommonFrontendServiceRegistration();
            services.PerformSecurityPersonalWebBackendServiceRegistration();
            services.PerformMediaWebCommonRelativePathFileManagementServiceRegistration();

            services.AddScoped<IEntityRepository<Framework.Guama.Core.Persistence.ORM.EF.Entities.Trace, long>, Framework.Guama.Core.Persistence.ORM.EF.Repositories.TraceRepository>();
            services.AddScoped<IEntityRepository<KeyWord, System.Guid>, Persistence.ORM.EF.Repositories.KeyWordRepository>();
            services.AddScoped<IEntityRepository<Map, string>, Persistence.ORM.EF.Repositories.MapRepository>();
            services.AddScoped<IEntityRepository<Section, System.Guid>, Persistence.ORM.EF.Repositories.SectionRepository>();
            services.AddScoped<IEntityRepository<Epigraph, System.Guid>, Persistence.ORM.EF.Repositories.EpigraphRepository>();
            services.AddScoped<IEntityRepository<Author, System.Guid>, Persistence.ORM.EF.Repositories.AuthorRepository>();
            services.AddScoped<IEntityRepository<Institution, System.Guid>, Persistence.ORM.EF.Repositories.InstitutionRepository>();

            services.AddSingleton<Framework.Guama.Core.Persistence.ORM.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade, Persistence.ORM.EF.Providers.DbSchemaConfigurationProvider>();

            services.AddSingleton<Framework.Guama.Core.Persistence.ORM.EF.Helpers.Facade.IDataInitializationHelper, Helpers.DataInitializationHelper>();

            services.AddHttpContextAccessor();

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(config =>
            {
                config.AccessDeniedPath = new PathString("/error/403");
                config.LoginPath = new PathString("/security/account/login");
                config.ExpireTimeSpan = System.TimeSpan.FromMinutes(5);
                config.SlidingExpiration = true;
            });

            #region Security concreter dependencies configuration
            services.AddSingleton<Framework.Guama.Core.Presentation.Web.Security.Helpers.Facade.ILicenseValidationHelperFacade, Helpers.WillcardLicenseValidationHelper>();

            services.AddTransient<Framework.Guama.Core.Presentation.Web.Security.Models.View.Contrat.IUserProfileViewModel, Models.View.UserProfileViewModel>();

            services.AddSingleton<Framework.Guama.Core.Presentation.Web.Common.ModelBinding.Contract.IConcretModelBindHelper, Helpers.ConcretModelBindHelper>();

            services.AddScoped<Framework.Guama.Core.Presentation.Web.Security.Managers.Facade.IUserProfileManagerFacade, Managers.UserProfileManager>();

            services.AddSingleton<Framework.Guama.Core.Presentation.Web.Security.Providers.Facade.IRoleIdProviderFacade>(Framework.Guama.Core.Security.Common.Presentation.Web.Frontend.Providers.RoleMemoryProvider.Instance);
            #endregion

            #region Globalization configuration
            var defaultCulture = new System.Globalization.CultureInfo("es");

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture);
                options.SupportedCultures = new[] { defaultCulture };
                options.SupportedUICultures = new[] { defaultCulture };

                options.RequestCultureProviders = new[] { new Framework.Guama.Core.Presentation.Web.Common.Localization.UrlRequestCultureProvider() };
            });

            services.AddSingleton<Helpers.GlobalViewLocalizationHelper>();
            #endregion

            services.Configure<Microsoft.AspNetCore.Routing.RouteOptions>(options => options.LowercaseUrls = true);

            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options => options.MultipartBodyLengthLimit = 209715200);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddSession();

            services.AddMvc(mvc =>
            {
                mvc.EnableEndpointRouting = false;
                mvc.ModelBinderProviders.Insert(0, new Framework.Guama.Core.Presentation.Web.Common.ModelBinding.AbstractsModelBinderProvider());
            })
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResources));
                }
            );

            services.AddDataProtection().PersistKeysToFileSystem(new System.IO.DirectoryInfo(System.IO.Path.Combine(env.ContentRootPath, "dpkeys")));

            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();
        }

        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions> requestLocalizationOptions, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, DbContext dbContext)
        {
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
            System.AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            loggerFactory.AddSerilog(Log.Logger);

            app.UseRequestLocalization(requestLocalizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging())
            {
                dbContext.Database.Migrate();

                var dataInitializationHelpers = app.ApplicationServices.GetServices<Framework.Guama.Core.Persistence.ORM.EF.Helpers.Facade.IDataInitializationHelper>();

                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var scopedDbContext = serviceScope.ServiceProvider.GetService<DbContext>();

                    foreach (var dataInitializationHelper in dataInitializationHelpers)
                    {
                        dataInitializationHelper.InitialiseData(scopedDbContext);
                    }
                }

                if (!(dbContext as Persistence.ORM.EF.ApplicationDbContext).Users.Any())
                {
                    var userId = configuration["DefaultUser:Id"];

                    var defaultUser = new Framework.Guama.Security.Personal.Persistence.Entities.User
                    {
                        Id = userId,
                        Password = configuration["DefaultUser:Password"].ToHashedPassword(),
                        Enabled = true,
                        HasFullAccess = true,
                        PasswordChangeRequired = true,
                        Identity = new Framework.Guama.Identity.Persistence.Entities.PersonIdentity
                        {
                            Name = userId,
                            Sex = Framework.Guama.Identity.Persistence.Enumerations.Sex.Unknown
                        }
                    };

                    defaultUser.Signature = Framework.Guama.Security.Personal.Persistence.ORM.EF.Helpers.Integrity.UserIntegrityHelper.GetSignature(defaultUser);

                    dbContext.Set<Framework.Guama.Security.Personal.Persistence.Entities.User>().Add(defaultUser);

                    (dbContext as Framework.Guama.Core.Persistence.ORM.EF.Context.Contract.AbstractTraceableDbContext).SaveChanges("system");
                }
            }
            else
            {
                app.UseExceptionHandler("/error/exception");

                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            var atStartUpRoles = dbContext.Set<Framework.Guama.Core.Security.Common.Persistence.Entities.Role>().Include(e => e.PermissionBridges).ThenInclude(b => b.Stern).ToArray();

            foreach (var item in atStartUpRoles)
            {
                Framework.Guama.Core.Security.Common.Presentation.Web.Frontend.Providers.RoleMemoryProvider.Instance.Roles.Add(item);
            }

            var contentTypePovider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

            contentTypePovider.Mappings.Add(".apk", "application/vnd.android.package-archive");

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = contentTypePovider
            });

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute("defaultAreaRoutes",
                    template: "{area:exists}/{controller=home}/{action=index}/{id?}");

                routes.MapRoute(
                    name: "defaultRoutes",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
