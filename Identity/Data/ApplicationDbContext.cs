using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityApp.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
			
		}

		public DbSet<Profile> Profiles { get; set; }



		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			

			builder.Entity<ApplicationUser>()
						.HasOne(u => u.Profile)
						.WithOne(p => p.User)
						.HasForeignKey<Profile>(p => p.UserId);

		}

		

		
	}
}
