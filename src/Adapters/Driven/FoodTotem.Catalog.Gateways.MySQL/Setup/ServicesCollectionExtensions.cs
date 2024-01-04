using FoodTotem.Catalog.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServicesCollectionExtensions
	{
		public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            services.AddDbContext<CatalogContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), o => {
                    o.SchemaBehavior(MySqlSchemaBehavior.Translate,
                        (schema, entity) => $"{schema ?? "dbo"}_{entity}");
                }));
        }
    }

    public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        private readonly string _connectionString = "Server=localhost;Port=3306;Database=foodtotem;Uid=user;Pwd=uSeRpAsSwOrD;";

        public CatalogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString), o => {
                o.SchemaBehavior(MySqlSchemaBehavior.Translate,
                    (schema, entity) => $"{schema ?? "dbo"}_{entity}");
            });

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}