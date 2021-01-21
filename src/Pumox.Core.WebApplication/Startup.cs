using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetAppCommon.BasicAuthentication.Services;
using NetAppCommon.BasicAuthentication.Services.Interface;
using NetAppCommon.Helpers.BasicAuthentication;
using Pumox.Core.Database.Data;
using Pumox.Core.Database.Models;

namespace Pumox.Core.WebApplication
{
    public class Startup
    {
        #region private readonly log4net.ILog _log4net
        /// <summary>
        /// Referencja klasy Log4NetLogget
        /// Reference to the Log4NetLogget class
        /// </summary>
        private readonly log4net.ILog _log4net = Log4netLogger.Log4netLogger.GetLog4netInstance(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            /// System sprawdzania poprawności w programie .NET Core 3,0 lub nowszy traktuje parametry niedopuszczające wartości null lub właściwości powiązane tak, jakby miały [Required] atrybut. - Wyłączenie
            services.AddControllersWithViews(
                options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            try
            {
                /// Kontekst bazy danych Vies.Core.Database.Data.ViesCoreDatabaseContext
                var pumoxCoreDatabaseAppSettings = new AppSettings();
                /// Kontekst bazy danych Vies.Core.Database.Data.ViesCoreDatabaseContext
                services.AddDbContextPool<PumoxCoreDatabaseContext>(
                    options => options.UseSqlServer(pumoxCoreDatabaseAppSettings.GetConnectionString(), element => element.EnableRetryOnFailure())
//#if DEBUG
//                    .EnableSensitiveDataLogging()
//                    .EnableDetailedErrors()
//                    .LogTo(Console.WriteLine)
//#endif
                    );
                _log4net.Info($"Plik ustawień: { pumoxCoreDatabaseAppSettings.FilePath }");
                _log4net.Info($"Baza danych: { pumoxCoreDatabaseAppSettings.GetConnectionString() }");
                _log4net.Info($"Data migracji: { pumoxCoreDatabaseAppSettings.LastMigrateDateTime }");
            }
            catch (Exception e)
            {
                _log4net.Error(string.Format("\n{0}\n{1}\n{2}\n{3}\n", e.GetType(), e.InnerException?.GetType(), e.Message, e.StackTrace), e);
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pumox.Core.WebApplication", Version = "v1" });
            });

            /// Configure basic authentication 
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            /// Configure DI for application services
            services.AddScoped<IUserService, UserService>();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pumox.Core.WebApplication v1"));
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
