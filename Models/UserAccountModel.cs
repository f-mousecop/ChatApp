using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class UserAccountModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string CurrentTime { get; set; }
    }
}
