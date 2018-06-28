using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactAdvantage.API.Configurations;
using ReactAdvantage.Data.EntityFramework;
using ReactAdvantage.Data.EntityFramework.GraphQlQuery;
using Swashbuckle.AspNetCore.Swagger;

namespace ReactAdvantage.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();

                services.ConfigureRepositories()
                    .AddMiddleware()
                    .AddCorsConfiguration()
                    .AddConnectionProvider(Configuration)
                    .AddAppSettings(Configuration)
                    .AddDbContext<ReactAdvantageContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddSwaggerGen(s =>
                {
                    s.SwaggerDoc("v1", new Info {Title = "ReactAdvantage API", Description = "ReactAdvantage API"});
                });
            services.AddTransient<GraphQlQuery>();
            //services.AddTransient<IUsersRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ReactAdvantageContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            }); 
                
                app.UseMvc();

                db.EnsureSeedData();
            

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 docs");
            });
        }
    }
}