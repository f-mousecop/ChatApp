using MySqlConnector;

namespace ChatApp.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server = localhost; Port = 3306; Database = chatapp_dev; User ID = chatapp; Password = dev_password123!; SslMode = None; AllowPublicKeyRetrieval = True; ";
        }
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
