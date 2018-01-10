using System;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using IdentityApp.Models;
using System.Threading.Tasks;

namespace IdentityApp
{
	public class SeedData
	{
		public static async Task EnsureSeedData(IServiceProvider serviceProvider)
		{
			Console.WriteLine("Seeding database...");

			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

				var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
				
				context.Database.Migrate();
				SeedIdentityServer(context);


				

				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				await SeedRoles(roleManager);
				await SeedUsers(userManager);
			}

			Console.WriteLine("Done seeding database.");
			Console.WriteLine();

			
		}

		private static void SeedIdentityServer(ConfigurationDbContext context)
		{

			if (!context.Clients.Any())
			{
				Console.WriteLine("Clients being populated");
				foreach (var client in Config.GetClients().ToList())
				{
					context.Clients.Add(client.ToEntity());
				}
				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("Clients already populated");
			}

			if (!context.IdentityResources.Any())
			{
				Console.WriteLine("IdentityResources being populated");
				foreach (var resource in Config.GetIdentityResources().ToList())
				{
					context.IdentityResources.Add(resource.ToEntity());
				}
				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("IdentityResources already populated");
			}

			if (!context.ApiResources.Any())
			{
				Console.WriteLine("ApiResources being populated");
				foreach (var resource in Config.GetApiResources().ToList())
				{
					context.ApiResources.Add(resource.ToEntity());
				}
				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("ApiResources already populated");
			}
		}

		private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			var roles = new List<string> { "Dev","Staff","Teacher","Student" };
			foreach (var item in roles)
			{
				await AddRoleIfNotExist(roleManager, item);
			}
			

		}

		private static async Task AddRoleIfNotExist(RoleManager<IdentityRole> roleManager, string roleName)
		{
			var role = await roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				await roleManager.CreateAsync(new IdentityRole { Name = roleName });
				
			}

			
		}

		private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
		{
			string email = "traders.com.tw@gmail.com";
			
			string role = "Dev";

			var user =await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				var devUser = new ApplicationUser
				{
					UserName = email,
					Email = email,

				};

				string password = "abcd1234";
				await userManager.CreateAsync(devUser, password);


				await userManager.AddToRoleAsync(devUser, role);

			}
			else
			{
				bool hasRole= await userManager.IsInRoleAsync(user, role);
				if (!hasRole) await userManager.AddToRoleAsync(user, role);
			}

			

			
		}
	}
}