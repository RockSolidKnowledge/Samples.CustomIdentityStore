using IdentityExpress.Manager.UI.Extensions;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace CustomDatabaseConnection
{
    // Note - this example will only work for SqlServer.
    internal class CustomConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string connectionString;

        public CustomConnectionFactory(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
             var key = "CustomConnectionFactoryConnectionString";
            connectionString = configuration.GetValue<string>(key);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception($"No connection string found in configuration setting \"{key}\"");
        }
        public DbConnection CreateAuditRecordsConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateDataProtectionConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateIdentityConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateIdentityServerConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateOperationalConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }
    }
}
