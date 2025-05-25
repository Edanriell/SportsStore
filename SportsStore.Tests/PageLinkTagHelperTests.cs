using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class PageLinkTagHelperTests
{
	[ Fact ]
	public void Can_Generate_Page_Links()
	{
		// Arrange
		var urlHelper = new Mock<IUrlHelper>();
		urlHelper.SetupSequence (
					expression : x =>
						x.Action (
								It.IsAny<UrlActionContext>()
							)
				).
			Returns (
					value : "Test/Page1"
				).
			Returns (
					value : "Test/Page2"
				).
			Returns (
					value : "Test/Page3"
				);

		var urlHelperFactory = new Mock<IUrlHelperFactory>();
		urlHelperFactory.Setup (
					expression : f =>
						f.GetUrlHelper (
								It.IsAny<ActionContext>()
							)
				).
			Returns (
					value : urlHelper.Object
				);

		var viewContext = new Mock<ViewContext>();

		var helper =
			new PageLinkTagHelper (
				helperFactory : urlHelperFactory.Object
			)
			{
				PageModel = new PagingInfo
							{
								CurrentPage = 2,
								TotalItems = 28,
								ItemsPerPage = 10
							},
				ViewContext = viewContext.Object,
				PageAction = "Test"
			};

		var ctx = new TagHelperContext (
				allAttributes : new TagHelperAttributeList(),
				items : new Dictionary<object, object>(),
				uniqueId : ""
			);

		var content = new Mock<TagHelperContent>();
		var output = new TagHelperOutput (
				tagName : "div",
				attributes : new TagHelperAttributeList(),
				getChildContentAsync : ( cache, encoder ) => Task.FromResult (
						result : content.Object
					)
			);

		// Act
		helper.Process (
				context : ctx,
				output : output
			);

		// Assert
		Assert.Equal (
				expected : @"<a href=""Test/Page1"">1</a>"
						 + @"<a href=""Test/Page2"">2</a>"
						 + @"<a href=""Test/Page3"">3</a>",
				actual : output.Content.GetContent()
			);
	}
}
