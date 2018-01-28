using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Permissions.Models;
using Permissions.DAL;

namespace Blog.DAL
{
	public class BlogContextSeed
	{
	
		public static void EnsureSeedData(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
			

				var context = scope.ServiceProvider.GetRequiredService<BlogContext>();
				context.Database.Migrate();

				scope.ServiceProvider.GetRequiredService<PermissionContext>().Database.Migrate();


				SeedCategories(context);

				var permissionContext = scope.ServiceProvider.GetRequiredService<PermissionContext>();
				SeedPermissions(permissionContext);

				
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
					Order = 7
				},
				new  Category
				{
					Active = true,
					Name = "校園日誌",
					Code = "diary",
					Order = 9
				},
				new  Category
				{
					Active = true,
					Name = "傑出校友",
					Code = "famer",
					Order = 5
				},
				new  Category
				{
					Active = true,
					Name = "大愛新聞",
					Code = "da-ai",
					Order = 3
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

		static void SeedPermissions(PermissionContext context)
		{
			var permissions = new List<Permission>
			{
				new  Permission
				{
					Name = "MANAGE_USERS",
					Title = "使用者管理",
					AdminOnly=true,
					Order=88
				},
				new  Permission
				{
					Name = "REVIEW_POSTS",
					Title = "文章審核",
					AdminOnly=false,
					Order=66
				},

				new  Permission
				{
					Name = "EDIT_POSTS",
					Title = "編輯文章",
					AdminOnly=false,
					Order=22
				},
				

			};

			foreach (var item in permissions)
			{
				CreatePermission(item, context);
			}

		}

		static void CreatePermission(Permission permission, PermissionContext context)
		{
			var exist = context.Permissons.Where(c => c.Name == permission.Name).FirstOrDefault();
			if (exist == null)
			{
				context.Permissons.Add(permission);
				context.SaveChanges();
			}
		}

		



	}

	
}
