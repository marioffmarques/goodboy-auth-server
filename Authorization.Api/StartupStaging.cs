using System.Linq;
using System.Reflection;
using Authorization.Api.MultiTenancy;
using Authorization.IdentityServer4.Repository;
using Authorization.Repository;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Authorization.Manager;
using Authorization.Email;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using IdentityServer4.Services;

namespace Authorization.Api
{
    public class StartupStaging
    {
        public IConfiguration Configuration { get; }

        public StartupStaging(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("auth_database_postgre");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AuthorizationDbContext>(options => options.UseNpgsql(connectionString));

            services.AddMvc(options => { options.Filters.Add(typeof(ValidateModelAttribute)); })
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            /*
             * Add Saaskit middleware to resolve Tenants on each request
             */
            services.AddMultitenancy<Tenant, AuthorizationTenantResolver>();


            /*
             * Configure .NET Identity Framework
             * MultitenantUserStore overrides the current .NET Identity Framework implementation in order to add support for multitenancy
             * Any operation that requires users information from DB will use this MultiTenantUserStore to fetch users from some specific Tenant
             * (For example: The operation of issuing a token for a user (grant_type=password) will use this MultiTenantUserStore to get users from Db
             */
            services.AddIdentity<User, Role>(IdentityConfig.ConfigureUserRequirements)
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddUserStore<MultiTenantUserStore<User>>()
            .AddDefaultTokenProviders();


            /*
             * Configure Identity Server
             */
            var IdSrv4builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = "http://authapi"; // Docker container URI //TODO - set container dns to avoid this
            })
            .AddAspNetIdentity<User>()
            .AddConfigurationStore(options => // Loads clients, identity resource, API resource, or CORS  from database store instead of InMemory store
            {
                options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options => // Loads grants, consents, and tokens from database store instead of InMemory store
            {
                options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });

            /*
             * Add IdentityServer Signing material to sign tokens - Get certficate
             */
            IdSrv4builder.AddSigningCredential(new X509Certificate2(Path.Combine(".", "authserver.pfx"), "1234"));

            // Get certificate from certificate store (KeyChain - "login")
            //IdSrv4builder.AddSigningCredential("CN=authserver", StoreLocation.CurrentUser);

            /*
             * Inject Application specific dependencies
             */
            AddApplicationDependentServices(services);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env /*, DBInitializer dbInitializer*/)
        {
            //dbInitializer.Seed(app).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMultitenancy<Tenant>();
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }


        private void AddApplicationDependentServices(IServiceCollection services)
        {
            services.AddTransient<DBInitializer>();
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.Configure<RecoverPasswordEmailSettings>(Configuration.GetSection("RecoverPasswordEmailSettings"));

            services.AddScoped<ITenantManager, TenantManager>();
            services.AddScoped<ITenantRepository, TenantRepository>();
        }
    }
}