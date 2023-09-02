using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MTodo.Persistance.Tables;


namespace MTodo.Persistance.Context
{
	public class MTodoContext:DbContext
	{
		public MTodoContext(DbContextOptions<MTodoContext> options) :base(options)
		{

		}
		public DbSet<Todo> todos { set; get; }
		public DbSet<User> users { set; get; }
	}
}

