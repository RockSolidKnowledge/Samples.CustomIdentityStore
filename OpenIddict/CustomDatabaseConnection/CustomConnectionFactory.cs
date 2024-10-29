using Microsoft.Data.SqlClient;
using System.Data.Common;
using IdentityExpress.Manager.BusinessLogic.Configuration.Database;

namespace CustomDatabaseConnection
{
    // Note - this example will only work for SqlServer.
    internal class CustomConnectionFactory : IOpenIddictDbConnectionFactory
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

        public DbConnection CreateDataProtectionConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateIdentityConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }

        public DbConnection CreateOpenIddictConnection(bool migration = false)
        {
            return new SqlConnection(connectionString);
        }
    }
}
