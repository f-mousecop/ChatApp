using MdXaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ChatApp.Utils
{
    /// <summary>
    /// Converts Markdown to FlowDocument using MdXaml
    /// </summary>
    public sealed class MarkdownRenderer
    {
        private readonly Markdown _md;

        public MarkdownRenderer()
        {
            _md = new Markdown
            {
                CodeStyle = (Style)Application.Current.FindResource("InlineCodeStyle")
            };
        }

        public FlowDocument Render(string markdown)
        {
            // Returns a fresh FlowDocument each call
            var doc = _md.Transform(markdown ?? string.Empty);

            doc.PagePadding = new Thickness(0);
            doc.FontSize = 14;

            // Make paragraphs looser
            foreach (var block in doc.Blocks)
            {
                if (block is Paragraph p)
                {
                    p.Margin = new Thickness(0, 0, 0, 8);
                }
            }

            return doc;
        }
    }
}
