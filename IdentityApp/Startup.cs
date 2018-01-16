using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityApp.Data;
using IdentityApp.Models;
using IdentityApp.Services;
using System.Reflection;
using IdentityApp.Areas.Admin.Data;
using IdentityApp.Areas.Admin.Services;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IdentityApp
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

			services.AddDbContext<ApplicationDbContext>(options =>
			   options.UseSqlServer(connectionString));

			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				// Password settings
				options.Password.RequireDigit = false;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;

			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options =>
			{
				options.Events.OnRedirectToLogin = ctx =>
				{
					if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
					{
						ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					}
					else
					{
						ctx.Response.Redirect(ctx.RedirectUri);
					}

					return Task.FromResult(0);
				};
			});

			var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;


			services.AddIdentityServer()
				.AddDeveloperSigningCredential()
				.AddAspNetIdentity<ApplicationUser>()
				.AddProfileService<IdentityProfileService>()
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				// this adds the operational data from DB (codes, tokens, consents)
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));

					// this enables automatic token cleanup. this is optional.
					options.EnableTokenCleanup = true;
					options.TokenCleanupInterval = 30;
				});



			services.AddTransient<IEmailSender, EmailSender>();


			services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddCors(options =>
			{
				// this defines a CORS policy called "default"
				options.AddPolicy("default", policy =>
				{
					policy.WithOrigins("http://localhost:50002", "http://localhost:50003")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});



			

			services.AddOptions();

			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


			services.AddScoped(typeof(IIdentityConfigRepository<>), typeof(IdentityConfigRepository<>));

			services.AddScoped<IIdentityConfigService, IdentityConfigService>();

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseCors("default");

			app.UseIdentityServer();

			app.UseStaticFiles();
			

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "areaRoute",
					template: "{area:exists}/{controller}/{action=Index}/{id?}");


				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		
	}
}
