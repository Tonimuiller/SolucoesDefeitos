using HtmlAgilityPack;

namespace SolucoesDefeitos.Pesentation.RazorPages.Extensions;

public static class HtmlExtensions
{
    public static string GetHtmlStringContent(this string htmlString)
    {
        if (htmlString == null) return string.Empty;
        string htmlSource = htmlString;
        var page = new HtmlDocument();
        page.LoadHtml(htmlSource);

        return string.Join(" ", page.DocumentNode.Descendants().
            Where(n =>
                n.NodeType == HtmlNodeType.Text &&
                n.ParentNode.Name != "script" &&
                n.ParentNode.Name != "style"
            ).Select(n => n.InnerText));
    }
}
