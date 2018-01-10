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
using Blog.DAL;

namespace BlogWeb
{
    public class Program
    {
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			BlogContextSeed.EnsureSeedData(host.Services);

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
