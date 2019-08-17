using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.AspNetCore.Localization;
using BruswBlog.Models;

namespace BruswBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration["Database:Type"] == "SQLite")
            {
                services.AddDbContext<BlogContext>(x => x.UseSqlite(Configuration["Database:ConnectionString"]));
            }
            else if (Configuration["Database:Type"] == "MySQL")
            {
                services.AddDbContext<BlogContext>(x => x.UseMySql(Configuration["Database:ConnectionString"]));
            }

            services.AddSmartCookies();

            services.AddMemoryCache();
            services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(20));

            services.AddBlobStorage()
                .AddEntityFrameworkStorage<BlogContext>()
                .AddSessionUploadAuthorization();

            services.AddPomeloLocalization(x =>
            {
                x.AddCulture(new string[] { "zh", "zh-CN", "zh-Hans", "zh-Hans-CN", "zh-cn" }, new JsonLocalizedStringStore(Path.Combine("Localization", "zh-CN.json")));
                x.AddCulture(new string[] { "en", "en-US", "en-GB" }, new JsonLocalizedStringStore(Path.Combine("Localization", "en-US.json")));
            });

            services.AddMvc()
                .AddMultiTemplateEngine()
                .AddCookieTemplateProvider();

            services.AddTimedJob();
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseBlobStorage("/assets/shared/scripts/jquery.codecomb.fileupload.js");

            app.UseMvcWithDefaultRoute();

            await SampleData.InitializeBruswBlog(app.ApplicationServices);

            app.UseTimedJob();
        }
    }
}
