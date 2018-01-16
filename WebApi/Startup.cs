
using IdentityApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IdentityApp.Services;

namespace WebApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddAuthorization().AddJsonFormatters();
			
			services.AddAuthentication("Bearer")
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = "http://localhost:50000";
					options.RequireHttpsMetadata = false;

					options.ApiName = "apiApp";


				});
			

			services.AddCors(options =>
			{
				// this defines a CORS policy called "default"
				options.AddPolicy("default", policy =>
				{
					policy.WithOrigins("http://localhost:50003", "http://localhost:50002")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});

			services.AddDbContext<ApplicationDbContext>(c =>
			{
				try
				{
					c.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));

			services.AddScoped<IUserService, UserService>();

		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseCors("default");

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				

				routes.MapRoute(
					name: "default",
					template: "api/{controller}/{action=Index}/{id?}");
			});
		}
	}

	
}
