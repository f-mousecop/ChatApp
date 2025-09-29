using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace ChatApp.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString =
                Environment.GetEnvironmentVariable("CHATAPP_MYSQL")
                ?? App.Configuration.GetConnectionString("ChatAppDb")
                ?? throw new InvalidOperationException("Database connection string not configured.");
        }
        protected MySqlConnection GetConnection() => new MySqlConnection(_connectionString);
    }
}
