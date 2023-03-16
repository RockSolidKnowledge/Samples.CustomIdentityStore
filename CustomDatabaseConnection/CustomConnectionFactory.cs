using IdentityExpress.Manager.UI.Extensions;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace CustomDatabaseConnection
{
    internal class CustomConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string connectionString = "Data Source=localhost\\SQLEXPRESS;User Id=AdminUI-User;Password=WoyblhD5G7gz7LA5Nz7e;Database=IdentityExpressDb;";
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
