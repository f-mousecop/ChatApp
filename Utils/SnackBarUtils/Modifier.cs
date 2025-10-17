using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatApp.Utils.SnackBarUtils
{
    public class Modifier : ModifierBase
    {
        public DependencyProperty Property { get; set; }
        public object Value { get; set; }
        public string TemplatePartName { get; set; }

        public override void Apply(DependencyObject target)
        {
            if (target is FrameworkElement element &&
                target.GetValue(Control.TemplateProperty) is ControlTemplate template &&
                template.FindName(TemplatePartName, element) is DependencyObject templatePart)
            {
                var valueToSet = ConvertIfNeeded(Value, Property?.PropertyType);
                templatePart.SetCurrentValue(Property, Value);
            }
        }

        private static object ConvertIfNeeded(object value, Type? targetType)
        {
            if (value == null || targetType == null) return value;

            // Already the right type
            if (targetType.IsInstanceOfType(value)) return value;

            // Try type converter (handles CornerRadius, Thickness, etc.)
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                if (value is string s && converter.CanConvertFrom(typeof(string)))
                    return converter.ConvertFromInvariantString(s);

                if (converter.CanConvertFrom(value.GetType()))
                    return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            }

            // Last resort: change type via Convert (works for numbers/bools)
            try { return System.Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture); }
            catch { return value; }
        }
    }
}
