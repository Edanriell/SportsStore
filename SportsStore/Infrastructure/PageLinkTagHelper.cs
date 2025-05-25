using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure;

[ HtmlTargetElement (
		tag : "div",
		Attributes = "page-model"
	) ]
public class PageLinkTagHelper : TagHelper
{
	private readonly IUrlHelperFactory urlHelperFactory;

	public PageLinkTagHelper ( IUrlHelperFactory helperFactory ) { urlHelperFactory = helperFactory; }

	[ ViewContext ]
	[ HtmlAttributeNotBound ]
	public ViewContext? ViewContext { get; set; }

	public PagingInfo? PageModel { get; set; }

	public string? PageAction { get; set; }

	[ HtmlAttributeName (
			DictionaryAttributePrefix = "page-url-"
		) ]
	public Dictionary<string, object> PageUrlValues { get; set; }
		= new();

	public bool PageClassesEnabled { get; set; } = false;
	public string PageClass { get; set; } = string.Empty;
	public string PageClassNormal { get; set; } = string.Empty;
	public string PageClassSelected { get; set; } = string.Empty;

	public override void Process ( TagHelperContext context,
								   TagHelperOutput output )
	{
		if ( ViewContext != null && PageModel != null )
		{
			var urlHelper
				= urlHelperFactory.GetUrlHelper (
						context : ViewContext
					);
			var result = new TagBuilder (
					tagName : "div"
				);
			for ( var i = 1;
				  i <= PageModel.TotalPages;
				  i++ )
			{
				var tag = new TagBuilder (
						tagName : "a"
					);
				PageUrlValues[key : "productPage"] = i;
				tag.Attributes[key : "href"] = urlHelper.Action (
						action : PageAction,
						values : PageUrlValues
					);
				if ( PageClassesEnabled )
				{
					tag.AddCssClass (
							value : PageClass
						);
					tag.AddCssClass (
							value : i == PageModel.CurrentPage
										? PageClassSelected
										: PageClassNormal
						);
				}

				tag.InnerHtml.Append (
						unencoded : i.ToString()
					);
				result.InnerHtml.AppendHtml (
						content : tag
					);
			}

			output.Content.AppendHtml (
					htmlContent : result.InnerHtml
				);
		}
	}
}
