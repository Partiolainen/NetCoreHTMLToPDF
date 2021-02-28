using System.IO;
using NetCoreHTMLToPDF.Enums;

namespace NetCoreHTMLToPDF.Console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var converter = new HtmlConverter();

            var html = "<div><strong>Hello</strong> World!</div>";
            var htmlBytes = converter.FromHtmlString(html, PageSize.A5, PageOrientation.Landscape, customFlags: "--encoding 'UTF-8'");
            File.WriteAllBytes("./html.pdf", htmlBytes);

            // From URL
            var urlBytes = converter.FromUrl("http://google.com", PageSize.Letter, PageOrientation.Landscape, customFlags: "--encoding 'UTF-8'");
            File.WriteAllBytes("./url.pdf", urlBytes);
        }
    }
}
