using Microsoft.EntityFrameworkCore;
using SOA.Models;

namespace SOA.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<UserModel> Users { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
	}
}
