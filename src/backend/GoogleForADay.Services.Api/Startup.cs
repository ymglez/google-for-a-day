using GoogleForADay.Core.Abstractions.Crawler;
using GoogleForADay.Core.Abstractions.Indexer;
using GoogleForADay.Core.Abstractions.Store;
using GoogleForADay.Core.Model.Store;
using GoogleForADay.Infrastructure.Crawler;
using GoogleForADay.Infrastructure.Indexer;
using GoogleForADay.Infrastructure.Store.LightningDB;
using GoogleForADay.Services.Api.Middleware;
using GoogleForADay.Services.Business.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleForADay.Services.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options => options.AddPolicy("AllowAllHeaders",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                })
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddSingleton<IKeyValueRepository<Keyword>, LightningRepository<Keyword>>()
                .AddTransient<IWebSiteCrawler, HtmlAgilityCrawler>()
                .AddSingleton<IPageIndexer, InvertedIndexer>()
                .AddSingleton<IndexerManagerBase, IndexerManager>()
                .AddScoped<SearchEngineController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy();
            app.UseCors("AllowAllHeaders");

            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMvc();
        }
    }
}
