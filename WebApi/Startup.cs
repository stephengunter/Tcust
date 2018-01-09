
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
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseAuthentication();

			app.UseMvc();
		}
	}
}
