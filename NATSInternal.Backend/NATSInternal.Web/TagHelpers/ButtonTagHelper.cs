using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;


[HtmlTargetElement("button", Attributes = "asp-for")]
public class ButtonTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    public ButtonTagHelper(IHtmlGenerator generator)
    {
        _generator = generator;
    }
    
    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For == null || string.IsNullOrEmpty(For.Name) || ViewContext is null)
        {
            return;
        }

        string name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
        string id = TagBuilder.CreateSanitizedId(name, _generator.IdAttributeDotReplacement);
        
        output.Attributes.SetAttribute("name", name);
        output.Attributes.SetAttribute("id", id);
    }
}