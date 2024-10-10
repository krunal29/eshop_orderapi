using RazorEngine.Templating;
using RazorEngine.Text;
using System;

namespace eshop_orderapi.Pdf
{
    public class PdfTemplateBase<T> : TemplateBase<T>
    {
        public static RawString MultiLine(string text)
        {
            return new RawString(new HtmlEncodedString(text).ToEncodedString().Replace(Environment.NewLine, "<br />"));
        }
    }
}