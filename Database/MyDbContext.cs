using Microsoft.EntityFrameworkCore;
using NumberReplacementApi.Constants;
using NumberReplacementApi.Models;
using System.Diagnostics;

namespace NumberReplacementApi.Database
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
		{
		}

		public DbSet<CalculationResult> CalculationResults { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) 
		{ 
			modelBuilder.Entity<CalculationResult>()
				.Property(e => e.Status)
				.HasConversion(
				       v => v.ToString(), 
					   v => (CalculationStatus)Enum.Parse(typeof(CalculationStatus), v)); 
		}
	}
}
