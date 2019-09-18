using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tcust.DAL;
using Microsoft.EntityFrameworkCore;
using Tcust.Services;
using Permissions.DAL;
using Permissions.Services;
using TcustApp.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace TcustApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {

			services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			services.AddOptions();

			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


			services.AddAuthorization(options =>
			{
				options.AddPolicy("ADMIN", policy =>
					policy.Requirements.Add(new HasPermissionRequirement("EDIT_POSTS")));
			});

			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
			.AddCookie("Cookies")
			.AddOpenIdConnect("oidc", options =>
			{
				options.SignInScheme = "Cookies";

				options.Authority = Configuration["AppSettings:AuthUrl"];
				options.RequireHttpsMetadata = false;

				options.ClientId = Configuration["AppSettings:AuthId"];
				options.ClientSecret = Configuration["AppSettings:AuthSecret"]; ;
				options.ResponseType = "code id_token";

				options.SaveTokens = true;
				options.GetClaimsFromUserInfoEndpoint = true;

				options.Scope.Add("apiApp");
				options.Scope.Add("profile");
				options.Scope.Add("offline_access");

				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					NameClaimType = "name",
					RoleClaimType = "role"
				};

			});

			


			services.AddDbContext<PermissionContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("TcustConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			services.AddDbContext<TcustContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("TcustConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			

			services.AddScoped(typeof(IPermissionRepository<>), typeof(PermissionRepository<>));
			services.AddScoped(typeof(ITcustRepository<>), typeof(TcustRepository<>));

			services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
			services.AddScoped<IPermissionService, PermissionService>();
			services.AddScoped<ITermService, TermService>();
            services.AddScoped<IDepartmentService, DepartmentService>();

        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

			app.UseAuthentication();

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areaRoute",
					template: "{area:exists}/{controller=Terms}/{action=Index}/{id?}");


				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
    }
}
