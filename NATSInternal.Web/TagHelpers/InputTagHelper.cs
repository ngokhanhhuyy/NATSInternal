using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NATSInternal.Web.TagHelpers;

[HtmlTargetElement("input", Attributes = "asp-for")]
public class DataTypeAttributeTagHelper : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For is null)
        {
            return;
        }
        
        Type type = For.ModelExplorer.ModelType;

        string? attributeValue;
        if (type == typeof(int))
        {
            attributeValue = "int";
        }
        else if (type == typeof(int?))
        {
            attributeValue = "int?";
        }
        else if (type == typeof(long))
        {
            attributeValue = "long";
        }
        else if (type == typeof(long?))
        {
            attributeValue = "long?";
        }
        else
        {
            return;
        }
        
        output.Attributes.SetAttribute("data-accept-type", attributeValue);
    }
}