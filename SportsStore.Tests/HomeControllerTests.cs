using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Tests;

public class HomeControllerTests
{
	[ Fact ]
	public void Can_Use_Repository()
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
									Name = "P1"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2"
								}
							}.AsQueryable()
				);

		var controller = new HomeController (
				repo : mock.Object
			);

		// Act
		var result =
			controller.Index (
							category : null
						)?.
					ViewData.Model
				as ProductsListViewModel
		 ?? new ProductsListViewModel();

		// Assert
		var prodArray = result.Products.ToArray();
		Assert.True (
				condition : prodArray.Length == 2
			);
		Assert.Equal (
				expected : "P1",
				actual : prodArray[0].Name
			);
		Assert.Equal (
				expected : "P2",
				actual : prodArray[1].Name
			);
	}

	[ Fact ]
	public void Can_Paginate()
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
									Name = "P1"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2"
								},
								new Product
								{
									ProductID = 3,
									Name = "P3"
								},
								new Product
								{
									ProductID = 4,
									Name = "P4"
								},
								new Product
								{
									ProductID = 5,
									Name = "P5"
								}
							}.AsQueryable()
				);

		var controller = new HomeController (
				repo : mock.Object
			);
		controller.PageSize = 3;

		// Act
		var result =
			controller.Index (
							category : null,
							productPage : 2
						)?.
					ViewData.Model
				as ProductsListViewModel
		 ?? new ProductsListViewModel();

		// Assert
		var prodArray = result.Products.ToArray();
		Assert.True (
				condition : prodArray.Length == 2
			);
		Assert.Equal (
				expected : "P4",
				actual : prodArray[0].Name
			);
		Assert.Equal (
				expected : "P5",
				actual : prodArray[1].Name
			);
	}


	[ Fact ]
	public void Can_Send_Pagination_View_Model()
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
									Name = "P1"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2"
								},
								new Product
								{
									ProductID = 3,
									Name = "P3"
								},
								new Product
								{
									ProductID = 4,
									Name = "P4"
								},
								new Product
								{
									ProductID = 5,
									Name = "P5"
								}
							}.AsQueryable()
				);

		// Arrange
		var controller =
			new HomeController (
				repo : mock.Object
			)
			{
				PageSize = 3
			};

		// Act
		var result =
			controller.Index (
							category : null,
							productPage : 2
						)?.
					ViewData.Model as
				ProductsListViewModel
		 ?? new ProductsListViewModel();

		// Assert
		var pageInfo = result.PagingInfo;
		Assert.Equal (
				expected : 2,
				actual : pageInfo.CurrentPage
			);
		Assert.Equal (
				expected : 3,
				actual : pageInfo.ItemsPerPage
			);
		Assert.Equal (
				expected : 5,
				actual : pageInfo.TotalItems
			);
		Assert.Equal (
				expected : 2,
				actual : pageInfo.TotalPages
			);
	}

	[ Fact ]
	public void Can_Filter_Products()
	{
		// Arrange
		// - create the mock repository
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
									Category = "Cat1"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2",
									Category = "Cat2"
								},
								new Product
								{
									ProductID = 3,
									Name = "P3",
									Category = "Cat1"
								},
								new Product
								{
									ProductID = 4,
									Name = "P4",
									Category = "Cat2"
								},
								new Product
								{
									ProductID = 5,
									Name = "P5",
									Category = "Cat3"
								}
							}.AsQueryable()
				);

		// Arrange - create a controller and make the page size 3 items
		var controller = new HomeController (
				repo : mock.Object
			);
		controller.PageSize = 3;

		// Action
		var result = ( controller.Index (
									   category : "Cat2",
									   productPage : 1
								   )?.
							   ViewData.Model
						   as ProductsListViewModel
					?? new ProductsListViewModel() ).Products.ToArray();

		// Assert
		Assert.Equal (
				expected : 2,
				actual : result.Length
			);
		Assert.True (
				condition : result[0].Name == "P2" && result[0].Category == "Cat2"
			);
		Assert.True (
				condition : result[1].Name == "P4" && result[1].Category == "Cat2"
			);
	}


	[ Fact ]
	public void Generate_Category_Specific_Product_Count()
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
									Category = "Cat1"
								},
								new Product
								{
									ProductID = 2,
									Name = "P2",
									Category = "Cat2"
								},
								new Product
								{
									ProductID = 3,
									Name = "P3",
									Category = "Cat1"
								},
								new Product
								{
									ProductID = 4,
									Name = "P4",
									Category = "Cat2"
								},
								new Product
								{
									ProductID = 5,
									Name = "P5",
									Category = "Cat3"
								}
							}.AsQueryable()
				);

		var target = new HomeController (
				repo : mock.Object
			);
		target.PageSize = 3;

		Func<ViewResult, ProductsListViewModel?> GetModel = result
			=> result?.ViewData?.Model as ProductsListViewModel;

		// Action
		var res1 = GetModel (
					arg : target.Index (
							category : "Cat1"
						)
				)?.
			PagingInfo.TotalItems;
		var res2 = GetModel (
					arg : target.Index (
							category : "Cat2"
						)
				)?.
			PagingInfo.TotalItems;
		var res3 = GetModel (
					arg : target.Index (
							category : "Cat3"
						)
				)?.
			PagingInfo.TotalItems;
		var resAll = GetModel (
					arg : target.Index (
							category : null
						)
				)?.
			PagingInfo.TotalItems;

		// Assert
		Assert.Equal (
				expected : 2,
				actual : res1
			);
		Assert.Equal (
				expected : 2,
				actual : res2
			);
		Assert.Equal (
				expected : 1,
				actual : res3
			);
		Assert.Equal (
				expected : 5,
				actual : resAll
			);
	}
}
