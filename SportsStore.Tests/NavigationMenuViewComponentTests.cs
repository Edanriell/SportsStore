using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;

namespace SportsStore.Tests;

public class NavigationMenuViewComponentTests
{
	[ Fact ]
	public void Can_Select_Categories()
	{
		// Arrange
		var mock = new Mock<IStoreRepository>();
		mock.Setup (
					expression : m => m.Products
				).
			Returns (
					value : new[]
							{
								new Product
								{
									ProductID = 1,
									Name = "P1",
									Category = "Apples"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2",
									Category = "Apples"
								},
								new Product
								{
									ProductID = 3,
									Name = "P3",
									Category = "Plums"
								},
								new Product
								{
									ProductID = 4,
									Name = "P4",
									Category = "Oranges"
								}
							}.AsQueryable()
				);

		var target =
			new NavigationMenuViewComponent (
					repo : mock.Object
				);

		// Act = get the set of categories
		var results = ( (IEnumerable<string>?)( target.Invoke()
													as ViewViewComponentResult )?.ViewData?.Model
					 ?? Enumerable.Empty<string>() ).ToArray();

		// Assert
		Assert.True (
				condition : new[]
							{
								"Apples",
								"Oranges",
								"Plums"
							}.SequenceEqual (
						second : results
					)
			);
	}

	[ Fact ]
	public void Indicates_Selected_Category()
	{
		// Arrange
		var categoryToSelect = "Apples";
		var mock = new Mock<IStoreRepository>();
		mock.Setup (
					expression : m => m.Products
				).
			Returns (
					value : new[]
							{
								new Product
								{
									ProductID = 1,
									Name = "P1",
									Category = "Apples"
								},
								new Product
								{
									ProductID = 4,
									Name = "P2",
									Category = "Oranges"
								}
							}.AsQueryable()
				);

		var target =
			new NavigationMenuViewComponent (
					repo : mock.Object
				);
		target.ViewComponentContext = new ViewComponentContext
									  {
										  ViewContext = new ViewContext
														{
															RouteData = new RouteData()
														}
									  };
		target.RouteData.Values[key : "category"] = categoryToSelect;

		// Action
		var result = (string?)( target.Invoke()
									as ViewViewComponentResult )?.ViewData?[index : "SelectedCategory"];

		// Assert
		Assert.Equal (
				expected : categoryToSelect,
				actual : result
			);
	}
}
