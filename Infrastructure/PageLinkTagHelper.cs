using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportHub.Models.ViewModels;

namespace SportHub.Infrastructure;

// describes target element - <div page-model=""></div>
[HtmlTargetElement("div", Attributes = "page-model")]

// TagHelper to render page links for pagination
public class PageLinkTagHelper(IUrlHelperFactory helperFactory) : TagHelper
{
    // framework injection mechanism - inject the current rendering context directly from the framework itself
    /* ViewContext contains information about:
    - which controller handled the request,
    - which action was called,
    - what route data exists,
    - what the current HTTP request looks like,
    - what URL generation context is available */
    
    // [HtmlAttributeNotBound] prevents Razor from looking for a "view-context" HTML attribute
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    public PageInfoViewModel? PageModel { get; set; }
    public string? PageAction { get; set; }

    public bool PageClassesEnabled { get; set; } = false;

    public string PageClass { get; set; } = string.Empty;

    public string PageClassNormal { get; set; } = string.Empty;

    public string PageClassSelected { get; set; } = string.Empty;

    
    // called automatically by Razor - context provides element metadata, output controls rendered HTML
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            IUrlHelper urlHelper = helperFactory.GetUrlHelper(ViewContext); // get URL generator bound to current routing context
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                }
                tag.Attributes["href"] = urlHelper.Action(PageAction,
                    new { productPage = i });
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }
            output.Content.AppendHtml(result.InnerHtml); // append only inner content
        }
    }
}