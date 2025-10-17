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
            DependencyProperty.Register(
                "Password",
                typeof(SecureString),
                typeof(SignupBindablePassBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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
            ApplyValidation(PasswordsMatch);
        }

        // DP to control borderbrush whether passwords match
        public bool PasswordsMatch
        {
            get => (bool)GetValue(PasswordsMatchProperty);
            set => SetValue(PasswordsMatchProperty, value);
        }

        public static readonly DependencyProperty PasswordsMatchProperty =
            DependencyProperty.Register(
                nameof(PasswordsMatch),
                typeof(bool),
                typeof(SignupBindablePassBox),
                new PropertyMetadata(false, OnPassWordsMatchChanged));

        private static void OnPassWordsMatchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (SignupBindablePassBox)d;
            ctrl.ApplyValidation((bool)e.NewValue);
        }

        private void ApplyValidation(bool matches)
        {
            // Get binding expression for the Password DP
            var be = GetBindingExpression(PasswordProperty);
            if (be == null) return;

            if (matches)
            {
                Validation.ClearInvalid(be);
            }
            else
            {
                // Add/replace validation error for ValidationAssist
                var error = new ValidationError(new DataErrorValidationRule(), be, "Passwords do not match", null);
                Validation.MarkInvalid(be, error);
            }
        }
    }
}
