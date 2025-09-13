using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public interface IUserRepository
    {
        Task<bool> AuthenticateUser(NetworkCredential credential);
        void Add(UserModel users);
        void Edit(UserModel users);
        void Remove(int id);
        UserModel GetByInt(int id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
    }
}
