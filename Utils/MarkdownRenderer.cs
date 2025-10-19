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
            _md = new Markdown { };
        }


        /// <summary>
        /// Render full markdown at once
        /// </summary>
        /// <param name="markdown"></param>
        /// <returns></returns>
        public FlowDocument Render(string markdown)
        {
            // Returns a fresh FlowDocument each call
            var doc = _md.Transform(markdown ?? string.Empty);

            PrepareDocumentDefaults(doc);
            LoosenParagraphs(doc);
            doc.PagePadding = new Thickness(0);
            doc.FontSize = 14;

            return doc;
        }


        /// <summary>
        /// Render markdown slice and return Block to append the existing FlowDocument
        /// </summary>
        /// <param name="markdownSlice"></param>
        /// <returns></returns>
        public List<Block> RenderToBlocks(string markdownSlice)
        {
            var tmp = _md.Transform(markdownSlice ?? string.Empty);
            LoosenParagraphs(tmp);

            var blocks = new List<Block>(tmp.Blocks.Count);
            while (tmp.Blocks.FirstBlock is Block first)
            {
                tmp.Blocks.Remove(first);
                blocks.Add(first);
            }

            return blocks;
        }


        /// <summary>
        /// Append a Markdown slice directly into an existing FlowDoc
        /// </summary>
        /// <param name="target"></param>
        /// <param name="markdownSlice"></param>
        public void AppendToDocument(FlowDocument target, string markdownSlice)
        {
            if (string.IsNullOrWhiteSpace(markdownSlice) || target is null) return;

            var blocks = RenderToBlocks(markdownSlice);
            foreach (var block in blocks)
            {
                target.Blocks.Add(block);
            }
        }

        private static void PrepareDocumentDefaults(FlowDocument doc)
        {
            doc.PagePadding = new Thickness(0);
            doc.FontSize = 14;
        }

        private static void LoosenParagraphs(FlowDocument doc)
        {
            foreach (var block in doc.Blocks)
            {
                if (block is Paragraph p)
                {
                    p.Margin = new Thickness(0, 0, 0, 8);
                }
            }
        }

    }
}
