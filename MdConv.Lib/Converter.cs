using Markdig;

namespace MdConv.Lib
{
    public class Converter
    {
        private const string KaTeXCssHtml = 
@"    <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/katex@0.16.4/dist/katex.min.css"" integrity=""sha384-vKruj+a13U8yHIkAyGgK1J3ArTLzrFGBbBc0tDp4ad/EyewESeXE/Iv67Aj8gKZ0"" crossorigin=""anonymous"">";
 
        private const string KaTeXJsHtml =
@"    <script defer src=""https://cdn.jsdelivr.net/npm/katex@0.16.4/dist/katex.min.js"" integrity=""sha384-PwRUT/YqbnEjkZO0zZxNqcxACrXe+j766U2amXcgMg5457rve2Y7I6ZJSm2A0mS4"" crossorigin=""anonymous""></script>
    <script defer src=""https://cdn.jsdelivr.net/npm/katex@0.16.4/dist/contrib/auto-render.min.js"" integrity=""sha384-+VBxd3r6XgURycqtZ117nYw44OOcIax56Z4dCRWbxyPt0Koah1uHoK0o4+/RRE05"" crossorigin=""anonymous""
        onload=""renderMathInElement(document.body);""></script>";

        private const string HtmlTemplate =
@"<!doctype html>
<html>
<head>
    <meta charset=""utf-8"">
    <title>@Title</title>
    <style type=""text/css"">
        @CSS
    </style>
    <style type=""text/css"">
        @PrismCss
    </style>
@KaTeXCss
</head>
<body class=""line-numbers"">
    <div class=""markdown"">
        @MarkdownHtml
    </div>
    <script>
        @PrismJs
    </script>
@KaTeXJs
</body>";

        public static string ToHTML(string markdowndoc, string title, string style, bool enableKaTeX)
        {
            return HtmlTemplate
                .Replace("@Title", title)
                .Replace("@MarkdownHtml", GenMarkdownHtml(markdowndoc))
                .Replace("@CSS", RetrieveCssFromRes(style))
                .Replace("@PrismCss", RetrievePrismCss())
                .Replace("@PrismJs", RetrievePrismJs())
                .Replace("@KaTeXCss", (enableKaTeX ? KaTeXCssHtml : string.Empty))
                .Replace("@KaTeXJs", (enableKaTeX ? KaTeXJsHtml : string.Empty));
        }

        private static string GenMarkdownHtml(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(md, pipeline);
        }

        private static string RetrieveCssFromRes(string style)
        {
            return CSSRes.ResourceManager.GetString(style) ?? CSSRes.Default;
        }

        private static string RetrieveCssFromExternal(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return CSSRes.Default;
            }
        }

        private static string RetrievePrismCss()
        {
            return MdConv.Lib.Properties.Resources.prism_css;
        }

        private static string RetrievePrismJs()
        {
            return MdConv.Lib.Properties.Resources.prism_js;
        }
    }
}