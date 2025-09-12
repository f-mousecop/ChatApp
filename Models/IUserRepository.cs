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
        bool AuthenticateUser(NetworkCredential credential);
        void Add(User users);
        void Edit(User users);
        void Remove(int id);
        User GetByInt(int id);
        User GetByUsername(string username);
        IEnumerable<User> GetByAll();
    }
}
