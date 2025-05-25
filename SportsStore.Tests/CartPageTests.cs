using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests;

public class CartPageTests
{
	[ Fact ]
	public void Can_Load_Cart()
	{
		// Arrange
		// - create a mock repository
		var p1 = new Product
				 {
					 ProductID = 1,
					 Name = "P1"
				 };
		var p2 = new Product
				 {
					 ProductID = 2,
					 Name = "P2"
				 };
		var mockRepo = new Mock<IStoreRepository>();
		mockRepo.Setup (
					expression : m => m.Products
				).
			Returns (
					value : new[]
							{
								p1,
								p2
							}.AsQueryable()
				);

		// - create a cart 
		var testCart = new Cart();
		testCart.AddItem (
				product : p1,
				quantity : 2
			);
		testCart.AddItem (
				product : p2,
				quantity : 1
			);

		// Action
		var cartModel = new CartModel (
				repo : mockRepo.Object,
				cartService : testCart
			);
		cartModel.OnGet (
				returnUrl : "myUrl"
			);

		// Assert
		Assert.Equal (
				expected : 2,
				actual : cartModel.Cart.Lines.Count()
			);
		Assert.Equal (
				expected : "myUrl",
				actual : cartModel.ReturnUrl
			);
	}


	[ Fact ]
	public void Can_Update_Cart()
	{
		// Arrange
		// - create a mock repository
		var mockRepo = new Mock<IStoreRepository>();
		mockRepo.Setup (
					expression : m => m.Products
				).
			Returns (
					value : new[]
							{
								new Product
								{
									ProductID = 1,
									Name = "P1"
								}
							}.AsQueryable()
				);

		var testCart = new Cart();

		// Action
		var cartModel = new CartModel (
				repo : mockRepo.Object,
				cartService : testCart
			);
		cartModel.OnPost (
				productId : 1,
				returnUrl : "myUrl"
			);

		// Assert
		Assert.Single (
				collection : testCart.Lines
			);
		Assert.Equal (
				expected : "P1",
				actual : testCart.Lines.First().
					Product.Name
			);
		Assert.Equal (
				expected : 1,
				actual : testCart.Lines.First().
					Quantity
			);
	}
}
