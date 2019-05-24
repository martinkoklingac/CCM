using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CcmClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserStore<CcmUser>, CcmUserRepository>();
            services.AddTransient<IRoleStore<CcmRole>, CcmRoleRepository>();

            services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Account/LogIn";
                    options.AccessDeniedPath = "/denied";
                });

            services.AddIdentity<CcmUser, CcmRole>()
                .AddDefaultTokenProviders();

            services.AddMvcCore()
                //.AddViews()
                .AddJsonFormatters()    //Provides JSON formatting in response
                .AddRazorViewEngine()
                .AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(rb => rb.MapRoute("default", "{controller=Index}/{action=Index}"));

            //NOTE : This will run after each request
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
