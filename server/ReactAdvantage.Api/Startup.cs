using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Models;
using ReactAdvantage.Api.Extensions;
using ReactAdvantage.Domain.Configuration;

namespace ReactAdvantage.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsEnvironment("Test"))
            {
                services.AddDbContext<ReactAdvantageContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "ReactAdvantage"));
            }
            else
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ReactAdvantageContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            services.AddIdentityCore<User, IdentityRole, ReactAdvantageContext>();

            // Add application services.
            services.AddGraphqlServices();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddMvc();

            var baseUrls = Configuration.GetBaseUrls();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = baseUrls.IdentityServer;
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "ReactAdvantageApi";
                });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(baseUrls.GraphqlPlaygroundJsClient)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app,
                                ILoggerFactory loggerFactory, IDbInitializer dbInitializer)
        {
            if (Environment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("default");

            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            dbInitializer.Initialize();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
