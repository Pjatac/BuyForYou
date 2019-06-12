using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public partial class BFUContext : DbContext
	{
		public BFUContext()
			: base("BFUContext")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer<BFUContext>(new DropCreateDatabaseIfModelChanges<BFUContext>());
			modelBuilder.Entity<Client>()
				.Property(e => e.FirstName)
				.IsUnicode(false);

			modelBuilder.Entity<Client>()
				.Property(e => e.LastName)
				.IsUnicode(false);

			modelBuilder.Entity<Client>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<Client>()
				.HasMany(e => e.Products)
				.WithRequired(e => e.Clients)
				.HasForeignKey(e => e.OwnerId)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Client>()
				.HasMany(e => e.Products1)
				.WithOptional(e => e.Clients1)
				.HasForeignKey(e => e.UserId);

			modelBuilder.Entity<Product>()
				.Property(e => e.Price)
				.HasPrecision(18, 0);
		}

		public virtual DbSet<Client> Clients { get; set; }
		public virtual DbSet<Product> Products { get; set; }
	}
}
