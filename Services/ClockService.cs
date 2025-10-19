using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ChatApp.Services
{
    public static class ClockService
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ClockService),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);
        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(string),
                typeof(ClockService),
                new PropertyMetadata("T"));

        public static void SetFormat(DependencyObject obj, string value) => obj.SetValue(FormatProperty, value);
        public static string GetFormat(DependencyObject obj) => (string)obj.GetValue(FormatProperty);

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBlock textBlock) return;

            if ((bool)e.NewValue)
            {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                void Tick(object? sender, EventArgs _) => textBlock.Text = DateTime.Now.ToString(GetFormat(textBlock));
                textBlock.SetValue(_timerProperty, timer);
                textBlock.Unloaded += Tb_Unloaded;
                timer.Tick += Tick;
                timer.Start();
                Tick(null, EventArgs.Empty);
            }
            else
            {
                Cleanup(textBlock);
            }
        }

        private static void Tb_Unloaded(object? sender, RoutedEventArgs e) => Cleanup((TextBlock)sender!);

        private static readonly DependencyProperty _timerProperty =
            DependencyProperty.RegisterAttached(
                "_timer",
                typeof(DispatcherTimer),
                typeof(ClockService),
                new PropertyMetadata(null));

        private static void Cleanup(TextBlock textBlock)
        {
            if (textBlock.GetValue(_timerProperty) is DispatcherTimer timer)
            {
                timer.Stop();
                timer.Tick -= null; // removes all handlers
            }
            textBlock.ClearValue(_timerProperty);
            textBlock.Unloaded -= Tb_Unloaded;
        }
    }
}
