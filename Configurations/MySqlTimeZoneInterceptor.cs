using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TravelMasterApi.Configurations
{
	public class MySqlTimeZoneInterceptor : DbConnectionInterceptor
	{
		public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
		{
			using var cmd = connection.CreateCommand();
			//cmd.CommandText = "SET time_zone = '+00:00';";
			cmd.CommandText = "SET time_zone = '+07:00';";
			await cmd.ExecuteNonQueryAsync(cancellationToken);
		}
	}
}
