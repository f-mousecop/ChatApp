using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace ChatApp.Views
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.Zero;
            tmr.Tick += Tmr_Tick;
            tmr.Start();
        }

        private void Tmr_Tick(object? sender, EventArgs e)
        {
            curr_Date_Time.Text = DateTime.UtcNow.ToLocalTime().ToString();
        }

        private void mainGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject d = e.OriginalSource as DependencyObject;
            while (d != null)
            {
                if (d is TextBox) return;
                d = System.Windows.Media.VisualTreeHelper.GetParent(d);
            }
            Keyboard.ClearFocus();
        }

        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (TextBox)sender;

            // If click bringing focus back to textbox
            // prevent default selection restore and place the caret where clicked
            if (!tb.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                tb.Focus();

                var p = e.GetPosition(tb);
                int index = tb.GetCharacterIndexFromPoint(p, true);
                if (index >= 0) tb.CaretIndex = index;
            }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.IsReadOnly) tb.Select(0, 0);
        }
    }
}
