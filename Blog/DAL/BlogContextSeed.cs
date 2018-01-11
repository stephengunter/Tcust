using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using ApplicationCore.Entities;

namespace Blog.DAL
{
	public class BlogContextSeed
	{
	
		public static void EnsureSeedData(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<BlogContext>();


				SeedCategories(context);


				SeedPermissions(context);

				
			}

		

		}

		static void SeedCategories(BlogContext context)
		{
			var categories = new List<Category>
			{

				new  Category
				{
					Active = true,
					Name = "榮譽榜",
					Code = "honor",
					Order = 3
				},
				new  Category
				{
					Active = true,
					Name = "校園日誌",
					Code = "diary",
					Order = 5
				},
				new  Category
				{
					Active = true,
					Name = "傑出校友",
					Code = "famer",
					Order = 1
				},
			};

			foreach (var item in categories)
			{
				CreateCategory(item, context);
			}
			
			 
		}

		static void CreateCategory(Category category, BlogContext context)
		{
			var exist = context.Categories.Where(c => c.Code == category.Code).FirstOrDefault();
			if (exist == null)
			{
				context.Categories.Add(category);
				context.SaveChanges();
			}
		}

		static void SeedPermissions(BlogContext context)
		{
			var permissions = new List<Permission>
			{

				new  Permission
				{
					Name = "EDIT_POSTS",
					Title = "編輯文章"
				},
				new  Permission
				{
					Name = "MANAGE_USERS",
					Title = "使用者管理"
				}
				
			};

			foreach (var item in permissions)
			{
				CreatePermission(item, context);
			}

		}

		static void CreatePermission(Permission permission, BlogContext context)
		{
			var exist = context.Permissions.Where(c => c.Name == permission.Name).FirstOrDefault();
			if (exist == null)
			{
				context.Permissions.Add(permission);
				context.SaveChanges();
			}
		}

		static void SeedAppUsers(BlogContext context)
		{
			string name = "secac11@tcust.edu.tw";
			string[] permissionNames= { "EDIT_POSTS", "MANAGE_USERS"};

			
			CreateAppUser(context , name, permissionNames);
		}

		static void CreateAppUser(BlogContext context, string name, string[] permissionNames)
		{
			var exist = context.AppUsers.Where(u => u.Name == name).FirstOrDefault();
			if (exist == null)
			{
				context.AppUsers.Add(new AppUser {
					Active =true ,
					CreatedAt=DateTime.Now,
					LastUpdated=DateTime.Now,
					Name= name,

				});
				context.SaveChanges();

				
			}

			AddUserPermissions(context, name, permissionNames);

		}

		static void AddUserPermissions(BlogContext context, string userName, string[] permissionNames)
		{

		}



	}

	
}
