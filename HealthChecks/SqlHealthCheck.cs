using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrdlyBackend.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly string _sqlQuery = "Select 1;";

        public SqlHealthCheck(string connectionString)
        {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);

                // Execute health check logic here.
                using (connection)
                {
                    await connection.OpenAsync(cancellationToken);

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = _sqlQuery;
                        await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }

            return HealthCheckResult.Healthy("Sql integration is healthy");
        }
    }
}
