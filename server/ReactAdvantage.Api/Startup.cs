using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Api.GraphQLSchema;
using ReactAdvantage.Data;

namespace ReactAdvantage.Api
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

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddDbContext<ReactAdvantageContext>(options =>
                options.UseSqlServer(connectionString));

            // Add application services.
            services.AddTransient<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<ReactAdvantageQuery>();
            //todo add mutation
            services.AddTransient<TaskType>();
            services.AddTransient<UserType>();
            services.AddTransient<ProjectType>();
            var sp = services.BuildServiceProvider();
            services.AddTransient<ISchema>(x => new ReactAdvantageSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                                ILoggerFactory loggerFactory, ReactAdvantageContext db)
        {
            if (env.IsDevelopment())
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

            app.UseAuthentication();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            db.EnsureSeedData();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
