using AndcultureCode.CSharp.Conductors;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using AndcultureCode.CSharp.Core.Interfaces.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sandbox.Business.Core.Interfaces;
using Sandbox.Infrastructure.Data.SqlServer;
using Sandbox.Infrastructure.Data.SqlServer.Extensions;
using Sandbox.Infrastructure.Data.SqlServer.Repositories;
using System.Text;

namespace Sandbox.Presentation.Web
{
    public class Startup
    {
        public IHostingEnvironment Environment   { get; }
        public IConfiguration Configuration { get; }

        public const string FRONTEND_DEVELOPMENT_SERVER_URL = "http://localhost:3000";

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment   = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins(FRONTEND_DEVELOPMENT_SERVER_URL).AllowAnyMethod().AllowAnyHeader();
            })); // Make sure you call this previous to AddMvc

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("some_big_key_value_here_secret")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<SandboxContext> (ServiceLifetime.Scoped);
            services.AddScoped                    ((sp) => new SandboxContext());
            services.AddScoped<IContext>          ((sp) => new SandboxContext());
            services.AddScoped<ISandboxContext>   ((sp) => new SandboxContext());

            // Repository defaults
            services.AddScoped(typeof(IRepositoryCreateConductor<>), typeof(RepositoryCreateConductor<>));
            services.AddScoped(typeof(IRepositoryReadConductor<>),   typeof(RepositoryReadConductor<>));
            services.AddScoped(typeof(IRepositoryUpdateConductor<>), typeof(RepositoryUpdateConductor<>));
            services.AddScoped(typeof(IRepositoryDeleteConductor<>), typeof(RepositoryDeleteConductor<>));
            services.AddScoped(typeof(IRepositoryConductor<>),       typeof(RepositoryConductor<>));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("ApiCorsPolicy");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name:     "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SandboxContext>();
                context.Database.Migrate();
                context.EnsureSeedData(Environment);
            }
        }
    }
}
