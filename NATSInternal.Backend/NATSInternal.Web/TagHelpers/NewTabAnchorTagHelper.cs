using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NATSInternal.Web.TagHelpers;

[HtmlTargetElement("a", Attributes = "asp-new-tab")]
public class NewTabAnchorTagHelper : TagHelper
{
    [HtmlAttributeName("asp-new-tab")]
    public bool AspNewTab { get; set; } = true;
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.SetAttribute("target", "_blank");
        if (!output.Attributes.ContainsName("rel"))
        {
            output.Attributes.SetAttribute("rel", "noopener noreferrer");
        }

        output.Attributes.RemoveAll("asp-new-tab");
    }
}