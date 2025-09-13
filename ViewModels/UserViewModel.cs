

namespace ChatApp.ViewModels
{
    /// <summary>
    /// UserViewModel that inherits from the BaseViewModel file 
    /// Defines ID and Username
    /// </summary>
    public class UserViewModel : BaseViewModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _email;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
    }
}
