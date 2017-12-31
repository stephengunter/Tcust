using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Tcust.DAL;
using Blog.DAL;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			var host = BuildWebHost(args);

			//SeedData(host);

			host.Run();

			
        }

		static void SeedData(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				var blogContext = services.GetRequiredService<BlogContext>();
				BlogContextSeed.Seed(blogContext);
			}


			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				var tcustContext = services.GetRequiredService<TcustContext>();
				TcustContextSeed.Seed(tcustContext);
			}
		}

		public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
