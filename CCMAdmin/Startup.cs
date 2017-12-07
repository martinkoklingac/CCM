using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CCMAdmin
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Prevent telemetry logs in Debug window
            TelemetryConfiguration.Active.DisableTelemetry = true;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(Configuration.GetSection("Logging"))
                    //.AddConsole()
                    .AddDebug();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUnitOfWorkConfig>((sp) => new UnitOfWorkConfig(this.Configuration.GetConnectionString("CCM")));
            services.AddSingleton<IUnitOfWorkProvider, UnitOfWorkProvider>();
            services.AddSingleton<IRegionService, RegionService>();
        }

        private static bool Check(HttpContext ctx)
        {
            var isCss = ctx.Request.Path.StartsWithSegments("/css");
            var isLib = ctx.Request.Path.StartsWithSegments("/lib");
            var isJs = ctx.Request.Path.StartsWithSegments("/js");
            var isImages = ctx.Request.Path.StartsWithSegments("/images");

            return !isCss
                && !isLib
                && !isJs
                && !isImages;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseWhen(Check, builder => builder.UseMiddleware<UnitOfWorkMiddleware>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
