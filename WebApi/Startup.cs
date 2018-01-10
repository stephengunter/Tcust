
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Blog.DAL;
using Blog.Services;
using Tcust.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi
{
    public class Startup
    {
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore()
				.AddAuthorization()
				.AddJsonFormatters();

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
					policy.WithOrigins("http://localhost:50003")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseCors("default");

			app.UseAuthentication();

			app.UseMvc();
		}
	}
}
