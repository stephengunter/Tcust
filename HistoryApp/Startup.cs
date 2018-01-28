using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using History.DAL.Tccn;
using Microsoft.EntityFrameworkCore;

namespace HistoryApp
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
			services.AddDbContext<TccnContext>(c =>
			{
				try
				{

					c.UseSqlServer(Configuration.GetConnectionString("TccnConnection"));
				}
				catch (System.Exception ex)
				{
					var message = ex.Message;
				}
			});

			services.AddMvc();
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
