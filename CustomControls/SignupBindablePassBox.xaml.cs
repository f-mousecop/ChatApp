using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApp.CustomControls
{
    /// <summary>
    /// Interaction logic for SignupBindablePassBox.xaml
    /// </summary>
    public partial class SignupBindablePassBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(SignupBindablePassBox));

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

        public SignupBindablePassBox()
        {
            InitializeComponent();
            signupPasswordBox.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = signupPasswordBox.SecurePassword;
        }
    }
}
