
using Tcust.Models;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tcust.DAL
{
	public class TcustContext : DbContext
	{
		public TcustContext(DbContextOptions<TcustContext> options) : base(options)
		{
		}

		public DbSet<CountryArea> CountryAreas { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Partner> Partners { get; set; }
		public DbSet<Contract> Contracts { get; set; }
		public DbSet<Type> Types { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ContractType>(ConfigureContractType);

			modelBuilder.Entity<PartnerContract>(ConfigurePartnerContract);

			modelBuilder.Entity<DepartmentContract>(ConfigureDepartmentContract);
		}

		private void ConfigureContractType(EntityTypeBuilder<ContractType> builder)
		{

			builder.HasKey(ct => new { ct.ContractId, ct.TypeId });

			builder.HasOne(ct => ct.Type)
				.WithMany("ContractTypes")
				.HasForeignKey(dc => dc.TypeId);


			builder.HasOne(dc => dc.Contract)
				.WithMany("ContractTypes")
				.HasForeignKey(dc => dc.ContractId);

		}

		private void ConfigurePartnerContract(EntityTypeBuilder<PartnerContract> builder)
		{

			builder.HasKey(pc => new { pc.PartnerId, pc.ContractId });

			builder.HasOne(pc => pc.Partner)
				.WithMany("PartnerContracts")
				.HasForeignKey(pc => pc.PartnerId);


			builder.HasOne(pc => pc.Contract)
				.WithMany("PartnerContracts")
				.HasForeignKey(pc => pc.ContractId);

		}

		private void ConfigureDepartmentContract(EntityTypeBuilder<DepartmentContract> builder)
		{

			builder.HasKey(dc => new { dc.DepartmentId, dc.ContractId });

			builder.HasOne(dc => dc.Department)
				.WithMany("DepartmentContracts")
				.HasForeignKey(dc => dc.DepartmentId);


			builder.HasOne(dc => dc.Contract)
				.WithMany("DepartmentContracts")
				.HasForeignKey(dc => dc.ContractId);

		}



	}
}
