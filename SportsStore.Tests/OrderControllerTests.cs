using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;

namespace SportsStore.Tests;

public class OrderControllerTests
{
	[ Fact ]
	public void Cannot_Checkout_Empty_Cart()
	{
		// Arrange - create a mock repository
		var mock = new Mock<IOrderRepository>();
		// Arrange - create an empty cart
		var cart = new Cart();
		// Arrange - create the order
		var order = new Order();
		// Arrange - create an instance of the controller
		var target = new OrderController (
				repoService : mock.Object,
				cartService : cart
			);

		// Act
		var result = target.Checkout (
							 order : order
						 ) as ViewResult;

		// Assert - check that the order hasn't been stored 
		mock.Verify (
				expression : m => m.SaveOrder (
						It.IsAny<Order>()
					),
				times : Times.Never
			);
		// Assert - check that the method is returning the default view
		Assert.True (
				condition : string.IsNullOrEmpty (
						value : result?.ViewName
					)
			);
		// Assert - check that I am passing an invalid model to the view
		Assert.False (
				condition : result?.ViewData.ModelState.IsValid
			);
	}

	[ Fact ]
	public void Cannot_Checkout_Invalid_ShippingDetails()
	{
		// Arrange - create a mock order repository
		var mock = new Mock<IOrderRepository>();
		// Arrange - create a cart with one item
		var cart = new Cart();
		cart.AddItem (
				product : new Product(),
				quantity : 1
			);
		// Arrange - create an instance of the controller
		var target = new OrderController (
				repoService : mock.Object,
				cartService : cart
			);
		// Arrange - add an error to the model
		target.ModelState.AddModelError (
				key : "error",
				errorMessage : "error"
			);

		// Act - try to checkout
		var result = target.Checkout (
							 order : new Order()
						 ) as ViewResult;

		// Assert - check that the order hasn't been passed stored
		mock.Verify (
				expression : m => m.SaveOrder (
						It.IsAny<Order>()
					),
				times : Times.Never
			);
		// Assert - check that the method is returning the default view
		Assert.True (
				condition : string.IsNullOrEmpty (
						value : result?.ViewName
					)
			);
		// Assert - check that I am passing an invalid model to the view
		Assert.False (
				condition : result?.ViewData.ModelState.IsValid
			);
	}


	[ Fact ]
	public void Can_Checkout_And_Submit_Order()
	{
		// Arrange - create a mock order repository
		var mock = new Mock<IOrderRepository>();
		// Arrange - create a cart with one item
		var cart = new Cart();
		cart.AddItem (
				product : new Product(),
				quantity : 1
			);
		// Arrange - create an instance of the controller
		var target = new OrderController (
				repoService : mock.Object,
				cartService : cart
			);

		// Act - try to checkout
		var result =
			target.Checkout (
					order : new Order()
				) as RedirectToPageResult;

		// Assert - check that the order has been stored
		mock.Verify (
				expression : m => m.SaveOrder (
						It.IsAny<Order>()
					),
				times : Times.Once
			);
		// Assert - check that the method is redirecting to the Completed action
		Assert.Equal (
				expected : "/Completed",
				actual : result?.PageName
			);
	}
}
