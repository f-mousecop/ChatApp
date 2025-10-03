using ChatApp.Models;
using ChatApp.Utils;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MySqlConnector;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security;

namespace ChatApp.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        // Auth
        public async Task<bool> AuthenticateUserAsync(string username, SecureString securePassword)
        {
            if (string.IsNullOrWhiteSpace(username) || securePassword is null || securePassword.Length < 3)
                return false;

            const string sql = "SELECT password_hash FROM users WHERE username = @username LIMIT 1;";

            await using var conn = GetConnection();
            await conn.OpenAsync();

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

            var result = await cmd.ExecuteScalarAsync();
            if (result is null) return false;

            var storedHash = (string)result;
            var plain = securePassword.ToUnsecureString();
            try
            {

                // Verify plain password against the stored hash
                var isValid = BCrypt.Net.BCrypt.Verify(plain, storedHash);
                return isValid;
            }
            finally { plain = string.Empty; }

        }

        public async Task AddAsync(UserModel users)
        {
            if (users is null) throw new ArgumentNullException(nameof(users));
            if (string.IsNullOrWhiteSpace(users.FirstName) ||
                string.IsNullOrWhiteSpace(users.LastName) ||
                string.IsNullOrWhiteSpace(users.Username) ||
                string.IsNullOrWhiteSpace(users.Email) ||
                string.IsNullOrWhiteSpace(users.MobileNumber) ||
                string.IsNullOrWhiteSpace(users.Password))
                throw new ArgumentException("All fields are required");


            // Normalize / trim
            users.FirstName = users.FirstName.Trim();
            users.LastName = users.LastName.Trim();
            users.Username = users.Username.Trim();
            users.Email = users.Email.Trim().ToLowerInvariant();
            users.MobileNumber = users.MobileNumber?.Trim() ?? "";
            users.AvatarUrl = users.AvatarUrl?.Trim() ?? "";


            const string existsSql = "SELECT COUNT(*) FROM users where username=@u or email=@e;";
            const string insertSql = @"
                INSERT INTO users (username, email, password_hash, first_name, last_name, avatar_url, mobile_number, is_email_verified)
                VALUES (@u, @e, @ph, @fn, @ln, @av, @mob, @ver);";

            var hash = BCrypt.Net.BCrypt.HashPassword(users.Password, workFactor: 12);

            await using var conn = GetConnection();
            await conn.OpenAsync();

            // Check for already existing user
            await using (var existsCmd = new MySqlCommand(existsSql, conn))
            {
                existsCmd.Parameters.Add("@u", MySqlDbType.VarChar).Value = users.Username;
                existsCmd.Parameters.Add("@e", MySqlDbType.VarChar).Value = users.Email;
                var count = (long)await existsCmd.ExecuteScalarAsync();
                if (count > 0) throw new InvalidOperationException("Username or email already taken");
            }

            try
            {
                // insert into db
                await using (var insertCmd = new MySqlCommand(insertSql, conn))
                {
                    insertCmd.Parameters.Add("@u", MySqlDbType.VarChar).Value = users.Username;
                    insertCmd.Parameters.Add("@e", MySqlDbType.VarChar).Value = users.Email;
                    insertCmd.Parameters.Add("@ph", MySqlDbType.VarChar).Value = hash;
                    insertCmd.Parameters.Add("@fn", MySqlDbType.VarChar).Value = users.FirstName;
                    insertCmd.Parameters.Add("@ln", MySqlDbType.VarChar).Value = users.LastName;
                    insertCmd.Parameters.Add("@av", MySqlDbType.VarChar).Value = string.IsNullOrWhiteSpace(users.AvatarUrl) ? DBNull.Value : users.AvatarUrl;
                    insertCmd.Parameters.Add("@mob", MySqlDbType.VarChar).Value = users.MobileNumber;
                    insertCmd.Parameters.Add("@ver", MySqlDbType.Bool).Value = users.IsEmailVerified;

                    await insertCmd.ExecuteNonQueryAsync();

                    // Mysqlconnector exposes LastInsertedId on Mysqlcommand 
                    // var newId = (int)insertCmd.LastInsertedId;
                    // return newId;
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062) // duplicates
            {
                throw new InvalidOperationException("Username or email already taken");
            }
        }


        public Task EditAsync(UserModel users)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<UserModel>> GetByAllAsync()
        {
            const string sql = @"
                SELECT
                    id, username, first_name, last_name, email, avatar_url, mobile_number, is_email_verified
                FROM users
                ORDER BY id;";

            var users = new List<UserModel>();

            await using var conn = GetConnection();
            await conn.OpenAsync();

            await using var cmd = new MySqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int idOrd = reader.GetOrdinal("id");
                int unOrd = reader.GetOrdinal("username");
                int fnOrd = reader.GetOrdinal("first_name");
                int lnOrd = reader.GetOrdinal("last_name");
                int emailOrd = reader.GetOrdinal("email");
                int avUrlOrd = reader.GetOrdinal("avatar_url");
                int mnumOrd = reader.GetOrdinal("mobile_number");
                int verifiedOrd = reader.GetOrdinal("is_email_verified");

                users.Add(new UserModel
                {
                    Id = reader.GetInt32(idOrd),
                    Username = reader.GetString(unOrd),
                    FirstName = reader.GetString(fnOrd),
                    LastName = reader.GetString(lnOrd),
                    Email = reader.GetString(emailOrd),
                    AvatarUrl = reader.IsDBNull(avUrlOrd) ? string.Empty : reader.GetString(avUrlOrd),
                    MobileNumber = reader.IsDBNull(mnumOrd) ? string.Empty : reader.GetString(mnumOrd),
                    IsEmailVerified = reader.GetBoolean(verifiedOrd),

                    Password = string.Empty
                });
            }
            return users;
        }


        public Task<UserModel?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel?> GetByIntAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            const string sql = @"SELECT id, username, email, first_name, last_name, avatar_url, mobile_number, is_email_verified
                                FROM users WHERE username=@u LIMIT 1;";

            await using var conn = GetConnection();
            await conn.OpenAsync();

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@u", MySqlDbType.VarChar).Value = username;

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new UserModel
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = reader["username"].ToString()!,
                Email = reader["email"].ToString()!,
                FirstName = reader["first_name"] is DBNull ? "" : reader["first_name"].ToString()!,
                LastName = reader["last_name"] is DBNull ? "" : reader["last_name"].ToString()!,
                AvatarUrl = reader["avatar_url"] is DBNull ? "" : reader["avatar_url"].ToString()!,
                MobileNumber = reader["mobile_number"] is DBNull ? "" : reader["mobile_number"].ToString()!,
                IsEmailVerified = reader["is_email_verified"] is DBNull ? false : Convert.ToBoolean(reader["is_email_verified"]),
                Password = string.Empty
            };
        }

        public async Task<int> RemoveAsync(int id)
        {
            const string sql = "DELETE FROM users WHERE id = @id;";

            await using var conn = GetConnection();
            await conn.OpenAsync();

            await using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            return await cmd.ExecuteNonQueryAsync();
        }

        public Task UpdateAvatarPathAsync(int id, string? avatarUrl)
        {
            throw new NotImplementedException();
        }
    }
}
