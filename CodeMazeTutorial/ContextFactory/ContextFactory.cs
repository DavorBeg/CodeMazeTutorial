using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CodeMazeTutorial.ContextFactory
{
	public class ContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
	{
		public RepositoryContext CreateDbContext(string[] args)
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.Development.json")
				.Build();

			var builder = new DbContextOptionsBuilder<RepositoryContext>()
				.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b =>
				{
					b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
				});

			return new RepositoryContext(builder.Options);
		}
	}
}
