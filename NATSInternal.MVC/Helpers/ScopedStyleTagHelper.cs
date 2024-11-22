using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NATSInternal.Helpers;

[HtmlTargetElement("style", Attributes = "scoped")]
public class ScopedStyleTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
    }
}
