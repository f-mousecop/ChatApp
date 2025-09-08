using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public UserViewModel User { get; } = new();
        public string ConfirmPass { get; set; }

    }
}
