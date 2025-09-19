using ChatApp.Models;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MySqlConnector;
using System.Net;

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
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM users WHERE username = @username LIMIT 1;";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Email = reader[2].ToString(),
                        };
                    }
                }
            }
            return user;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
