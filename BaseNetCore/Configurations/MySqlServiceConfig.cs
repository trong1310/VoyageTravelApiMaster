
using Microsoft.EntityFrameworkCore;
using TravelMasterApi.Databases;


namespace TravelMasterApi.Configurations
{
    public static class MySqlServiceConfig
    {
        private static string MySqlString { get; set; } = string.Empty;
		public static void ConfigureMySql(this IServiceCollection _services, string connectionString)
		{
			MySqlString = connectionString;
			_services.AddDbContext<MasterDBContext>(o =>
			{
				// Providing details log on DataBase error
				o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), x => x.UseNetTopologySuite());
				// Tắt log chi tiết
				o.EnableDetailedErrors();
				o.EnableSensitiveDataLogging();
				o.AddInterceptors(new MySqlTimeZoneInterceptor());
			});
		}
		public static MasterDBContext GetDbContext()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MasterDBContext>();
			optionsBuilder.UseMySql(MySqlString, ServerVersion.AutoDetect(MySqlString));
			optionsBuilder.EnableDetailedErrors();

			var context = new MasterDBContext(optionsBuilder.Options);
			context.Database.OpenConnection();

			return context;
		}

	}
}
