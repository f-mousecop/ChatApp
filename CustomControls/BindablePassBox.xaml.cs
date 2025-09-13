using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp.CustomControls
{
    /// <summary>
    /// Interaction logic for BindablePassBox.xaml
    /// </summary>
    public partial class BindablePassBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePassBox));

        public SecureString Password
        {
            get
            {
                return (SecureString)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        public BindablePassBox()
        {
            InitializeComponent();
            passwordBox.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.SecurePassword;
        }
    }
}
