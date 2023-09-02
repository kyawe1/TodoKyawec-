using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MTodo.Persistance
{
	public static class Injector
	{
		public static void AddPostgresql(this IServiceCollection services,string connectionString)
		{
			services.AddDbContext<Context.MTodoContext>(options => options.UseNpgsql(connectionString));
		}
	}
}

