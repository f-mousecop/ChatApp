using ChatApp.Models;
using System.Net;
using MySqlConnector;

namespace ChatApp.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel users)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateUser(NetworkCredential credential)
        {
            bool validateUser;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT password_hash FROM users WHERE username = @username LIMIT 1;";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;
                validateUser = command.ExecuteScalar() == null ? false : true;
            }
            return Task.FromResult(validateUser);
        }

        public void Edit(UserModel users)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public UserModel GetByInt(int id)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
