using System;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Data;
using ReactAdvantage.Domain.Configuration;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.IdentityServer.Startup
{
    public class Startup
    {
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Environment = environment;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            if (Environment.IsEnvironment("Test"))
            {
                services.AddDbContext<ReactAdvantageContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "ReactAdvantage"));
            }
            else
            {
                services.AddDbContext<ReactAdvantageContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ReactAdvantageContext>()
                .AddDefaultTokenProviders();

            //services.AddSingleton<BaseUrls>();

            services.AddMvc();

            var identityServerBuilder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(Configuration.GetBaseUrls(), Environment))
                .AddAspNetIdentity<User>()
                .AddOperationalStore(options =>
                {
                    if (Environment.IsEnvironment("Test"))
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseInMemoryDatabase("ReactAdvantage");
                    }
                    else
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(connectionString,
                                sql => sql.MigrationsAssembly(migrationsAssembly));
                    }

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                });

            var signingCertificateThumbprint = Configuration["IdentityServer:SigningCertificateThumbprint"];
            _logger.LogInformation($"IdentityServer:SigningCertificateThumbprint is '{signingCertificateThumbprint}'");

            if (string.IsNullOrEmpty(signingCertificateThumbprint) && (Environment.IsDevelopment() || Environment.IsEnvironment("Test")))
            {
                _logger.LogInformation("Using developer signing credential for Identity Server");
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                if (string.IsNullOrEmpty(signingCertificateThumbprint))
                {
                    throw new Exception("Signing certificate wasn't specified. Please add the certificate to the store and provide its thumbprint in the 'IdentityServer:SigningCertificateThumbprint' setting");
                }

                var certificate = CertificateHelper.GetCertificateFromStore(signingCertificateThumbprint);
                if (certificate == null)
                {
                    throw new Exception("Signing certificate with the specified thumbprint wasn't found in the store");
                }

                _logger.LogInformation("Using a certificate for signing");
                identityServerBuilder.AddSigningCredential(certificate);
            }

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            InitializeDatabase(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

                if (Environment.IsEnvironment("Test"))
                {
                    _logger.LogInformation("Creating database if doesn't exist");
                    db.Database.EnsureCreated();
                }
                else
                {
                    _logger.LogInformation("Migrating database");
                    db.Database.Migrate();
                }
            }
        }
    }
}
