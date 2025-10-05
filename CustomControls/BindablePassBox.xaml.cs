using ChatApp.Utils;
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
        // Prevent event feedback loops
        private bool _syncing;

        // Two-way dp so the LoginViewModel can bind SecureString
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(SecureString),
                typeof(BindablePassBox),
                new FrameworkPropertyMetadata(default(SecureString), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordChanged));



        // Bool flag to show/hide PasswordBox/textbox
        public static readonly DependencyProperty IsPasswordVisibleProperty =
            DependencyProperty.Register(
                nameof(IsPasswordVisible),
                typeof(bool),
                typeof(BindablePassBox),
                new PropertyMetadata(false, OnIsPasswordVisibleChanged));



        public SecureString Password
        {
            get => (SecureString)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public bool IsPasswordVisible
        {
            get => (bool)GetValue(IsPasswordVisibleProperty);
            set => SetValue(IsPasswordVisibleProperty, value);
        }

        public BindablePassBox()
        {
            InitializeComponent();
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            clearBox.TextChanged += ClearBox_TextChanged;
            UpdateVisualStyle();
        }



        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (BindablePassBox)d;
            if (ctrl._syncing) return;

            try
            {
                ctrl._syncing = true;

                // Update both boxes from SecureString
                var newSecure = e.NewValue as SecureString;
                string plain = newSecure?.ToUnsecureString() ?? string.Empty;

                ctrl.passwordBox.Password = plain ?? string.Empty;
                ctrl.clearBox.Text = plain ?? string.Empty;
            }
            finally { ctrl._syncing = false; }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.SecurePassword;
        }

        // VM -> Control visibility flag changed
        private static void OnIsPasswordVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (BindablePassBox)d;
            ctrl.UpdateVisualStyle();
        }


        private void UpdateVisualStyle()
        {
            _syncing = true;

            if (IsPasswordVisible)
            {
                // moving to clear text view: copy from masked
                clearBox.Text = passwordBox.Password;
                clearBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                // moving to masked view: copy from clear
                passwordBox.Password = clearBox.Text;
                clearBox.Text = string.Empty;
                passwordBox.Visibility = Visibility.Visible;
                clearBox.Visibility = Visibility.Collapsed;
            }
            // keep dependency property in sync
            Password = passwordBox.SecurePassword;
            _syncing = false;
        }

        // User typed in masked box
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_syncing) return;
            try
            {
                _syncing = true;
                clearBox.Text = passwordBox.Password; // mirror to clear text
                Password = passwordBox.SecurePassword; // update dependency property
            }
            finally { _syncing = false; }
        }

        // User typed in clear box
        private void ClearBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_syncing) return;
            try
            {
                _syncing = true;
                passwordBox.Password = clearBox.Text; // mirror to masked
                Password = passwordBox.SecurePassword; // update dependency property
            }
            finally { _syncing = false; }
        }


    }
}
