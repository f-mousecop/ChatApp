using System.Net;
using System.Security;

namespace ChatApp.Models
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUserAsync(string username, SecureString securePassword);
        Task AddAsync(UserModel users);
        Task<UserModel?> GetByUsernameAsync(string username);
        Task<UserModel?> GetByIntAsync(int id);
        Task<UserModel?> GetByEmailAsync(string email);
        Task UpdateAvatarPathAsync(int id, string? avatarUrl);
        Task EditAsync(UserModel users);
        Task<int> RemoveAsync(int id);
        Task<IReadOnlyList<UserModel>> GetByAllAsync();
    }
}
