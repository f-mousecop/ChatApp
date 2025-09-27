using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatApp.ValueConverters
{
    public class StateToVisConverter : BaseValueConverter<StateToVisConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not WindowState state) return Visibility.Hidden;

            // Parse params
            string param = parameter as string ?? "Normal";
            string[] parts = param.Split('|', StringSplitOptions.RemoveEmptyEntries);
            string desiredStateName = parts.Length > 0 ? parts[0] : "Normal";
            bool useCollapsed = parts.Length > 1 && parts[1].Equals("Collapsed", StringComparison.OrdinalIgnoreCase);

            // Map desired state
            WindowState desiredState = desiredStateName.ToLowerInvariant() switch
            {
                "normal" => WindowState.Normal,
                "maximized" => WindowState.Maximized,
                "minimized" => WindowState.Minimized,
                _ => WindowState.Normal
            };

            bool isVisible = state == desiredState;
            return isVisible ? Visibility.Visible : (useCollapsed ? Visibility.Collapsed : Visibility.Hidden);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
