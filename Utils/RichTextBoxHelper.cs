using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChatApp.Utils
{
    public static class RichTextBoxHelper
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.RegisterAttached(
                "Document",
                typeof(FlowDocument),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(null, OnDocumentChanged));

        public static void SetDocument(DependencyObject element, FlowDocument value)
            => element.SetValue(DocumentProperty, value);

        public static FlowDocument GetDocument(DependencyObject element)
            => (FlowDocument)element.GetValue(DocumentProperty);

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RichTextBox richTextBox) return;

            // RichTextBox.Document can't be null, always assign something
            richTextBox.Document = e.NewValue as FlowDocument ?? new FlowDocument();

            // Autoscroll
            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            richTextBox.ScrollToEnd();
        }
    }
}
