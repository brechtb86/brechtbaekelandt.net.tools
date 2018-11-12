using Microsoft.AspNetCore.Html;

namespace brechtbaekelandt.tools.Extensions
{
    public static class HtmlStringExtensions
    {
        public static HtmlString ToEscapedJSHtmlString(this HtmlString htmlString)
        {
            return new HtmlString($@"{htmlString.Value.Replace("/", "\\/")}");
        }
    }
}
