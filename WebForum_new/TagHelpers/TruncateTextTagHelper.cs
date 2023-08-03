using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebForum_new.TagHelpers;

[HtmlTargetElement("truncate")]
public class TruncateTextTagHelper : TagHelper
{
    [HtmlAttributeName("text")]
    public string Text { get; set; }

    [HtmlAttributeName("after-truncate")]
    public string AfterTruncate { get; set; } = "...";

    [HtmlAttributeName("length")]
    public int Length { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = null;
            output.Content.SetHtmlContent(HtmlEncoder.Default.Encode(TruncateText(Text, Length, AfterTruncate)));
        }
    }

    private static string TruncateText(string text, int maxLength, string afterTruncate)
    {
        return text.Length <= maxLength ? text : $"{text.Substring(0, maxLength)}{afterTruncate}";
    }
}