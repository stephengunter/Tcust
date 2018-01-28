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

				scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

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
			string username = "ss355";
			string fullname = "何金水";
			string role = "Dev";
			string password = "secret";

			await CreateUserIfNotExist(userManager, username, fullname, role, password);



			username = "ss123";
			fullname = "何金銀";
			role = "Staff";
		

			await CreateUserIfNotExist(userManager, username, fullname, role, password);

			username = "ss888";
			fullname = "高小琴";
			role = "Student";
		

			await CreateUserIfNotExist(userManager, username, fullname, role, password);

		}


		private static async Task CreateUserIfNotExist(UserManager<ApplicationUser> userManager, string username, string fullname, string role, string password)
		{

			var user = await userManager.FindByNameAsync(username);
			if (user == null)
			{
				
				var newUser = new ApplicationUser
				{
					UserName = username,
					Email = String.Format("{0}@tcust.edu.tw", username),

					CreatedAt = DateTime.Now,
					LastUpdated = DateTime.Now,
					SecurityStamp = Guid.NewGuid().ToString(),

					
					Profile = new Profile
					{
						Fullname= fullname,
						CreatedAt = DateTime.Now,
						DOB = new DateTime(1980,1,1),
						LastUpdated= DateTime.Now,
						Gender=true,
						  
					}

				};


				var result = await userManager.CreateAsync(newUser, password);

				
				await userManager.AddToRoleAsync(newUser, role);

				

			}
			else
			{
				bool hasRole = await userManager.IsInRoleAsync(user, role);
				if (!hasRole) await userManager.AddToRoleAsync(user, role);
			}
		}


		


	}
}