using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Authorization.IdentityServer4.Repository;
using Authorization.Manager;
using Authorization.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authorization.Resources.Api
{
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
        {
            AutoMapperConfig.Initialize();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("auth_database_postgre");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Text.Encoding.GetEncoding("Cyrillic");

            services.AddMvcCore(options => { options.Filters.Add(typeof(ValidateModelAttribute)); })
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    })
                    .AddAuthorization()
                    .AddJsonFormatters();



            services.AddDbContext<AuthorizationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddIdentity<User, Role>(IdentityConfig.ConfigureUserRequirements)
                    .AddEntityFrameworkStores<AuthorizationDbContext>();
                    //.AddUserStore<MultiTenantUserStore<User>>(); 

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                options.Authority = Configuration["AuthServer"];
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "authorizationresourcesapi";
                });

            var IdSrv4builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            //.AddAspNetIdentity<User>()
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
             * Inject Application specific dependencies
             */
            AddApplicationDependentServices(services);
        }

        private void AddApplicationDependentServices(IServiceCollection services)
        {
            /*
             *  Managers
             */
            services.AddScoped<ITenantManager, TenantManager>();
            services.AddScoped<IClientManager, ClientManager>();
            services.AddScoped<IApiResourceManager, ApiResourceManager>();

            /*
             *  Repositories
             */
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
