using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NATSInternal.Web.TagHelpers;

[HtmlTargetElement("div", Attributes = "asp-form-field-for")]
public class FormFieldTagHelper : TagHelper
{
    [HtmlAttributeName("asp-form-field-for")]
    public ModelExpression? For { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For == null || string.IsNullOrEmpty(For.Name))
        {
            return;
        }

        string propertyPath = For.Name;
        string idValue = "form-field-" + propertyPath.Replace('.', '_').Replace('[', '_').Replace(']', '_');

        output.Attributes.SetAttribute("id", idValue);
    }
}