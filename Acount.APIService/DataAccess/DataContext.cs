using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Acount.APIService.DataAccess
{
	public class PGDBContext : DbContext
	{
		public PGDBContext(DbContextOptions<PGDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//builder.Entity<FreqAskedQuestionsEntity>().Property(d => d.FAQDetails).HasColumnType("json");
			//builder.Entity<FreqAskedQuestionsEntity>().Property(d => d.FAQDetailsMobile).HasColumnType("json");
		}
		public DbSet<UserMasterEntity> userMasterEntity { get; set; }
		public DbSet<SmtpConfiguration> smtpConfiguration { get; set; }

	}

	public class BusinessDBContext
	{
		public static IServiceProvider serviceProvider;
		public PGDBContext GetDataContext()
		{
			var options = serviceProvider.GetService<DbContextOptions<PGDBContext>>();
			return new PGDBContext(options);
		}
	}
}
