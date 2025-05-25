using SportsStore.Models;

namespace SportsStore.Tests;

public class CartTests
{
	[ Fact ]
	public void Can_Add_New_Lines()
	{
		// Arrange - create some test products
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

		// Arrange - create a new cart
		var target = new Cart();

		// Act
		target.AddItem (
				product : p1,
				quantity : 1
			);
		target.AddItem (
				product : p2,
				quantity : 1
			);
		var results = target.Lines.ToArray();

		// Assert
		Assert.Equal (
				expected : 2,
				actual : results.Length
			);
		Assert.Equal (
				expected : p1,
				actual : results[0].Product
			);
		Assert.Equal (
				expected : p2,
				actual : results[1].Product
			);
	}

	[ Fact ]
	public void Can_Add_Quantity_For_Existing_Lines()
	{
		// Arrange - create some test products
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

		// Arrange - create a new cart
		var target = new Cart();

		// Act
		target.AddItem (
				product : p1,
				quantity : 1
			);
		target.AddItem (
				product : p2,
				quantity : 1
			);
		target.AddItem (
				product : p1,
				quantity : 10
			);
		var results = ( target.Lines ?? new List<CartLine>() ).OrderBy (
					keySelector : c => c.Product.ProductID
				).
			ToArray();

		// Assert
		Assert.Equal (
				expected : 2,
				actual : results.Length
			);
		Assert.Equal (
				expected : 11,
				actual : results[0].Quantity
			);
		Assert.Equal (
				expected : 1,
				actual : results[1].Quantity
			);
	}

	[ Fact ]
	public void Can_Remove_Line()
	{
		// Arrange - create some test products
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
		var p3 = new Product
				 {
					 ProductID = 3,
					 Name = "P3"
				 };

		// Arrange - create a new cart
		var target = new Cart();
		// Arrange - add some products to the cart
		target.AddItem (
				product : p1,
				quantity : 1
			);
		target.AddItem (
				product : p2,
				quantity : 3
			);
		target.AddItem (
				product : p3,
				quantity : 5
			);
		target.AddItem (
				product : p2,
				quantity : 1
			);

		// Act
		target.RemoveLine (
				product : p2
			);

		// Assert
		Assert.Empty (
				collection : target.Lines.Where (
						predicate : c => c.Product == p2
					)
			);
		Assert.Equal (
				expected : 2,
				actual : target.Lines.Count()
			);
	}


	[ Fact ]
	public void Calculate_Cart_Total()
	{
		// Arrange - create some test products
		var p1 = new Product
				 {
					 ProductID = 1,
					 Name = "P1",
					 Price = 100M
				 };
		var p2 = new Product
				 {
					 ProductID = 2,
					 Name = "P2",
					 Price = 50M
				 };

		// Arrange - create a new cart
		var target = new Cart();

		// Act
		target.AddItem (
				product : p1,
				quantity : 1
			);
		target.AddItem (
				product : p2,
				quantity : 1
			);
		target.AddItem (
				product : p1,
				quantity : 3
			);
		var result = target.ComputeTotalValue();

		// Assert
		Assert.Equal (
				expected : 450M,
				actual : result
			);
	}

	[ Fact ]
	public void Can_Clear_Contents()
	{
		// Arrange - create some test products
		var p1 = new Product
				 {
					 ProductID = 1,
					 Name = "P1",
					 Price = 100M
				 };
		var p2 = new Product
				 {
					 ProductID = 2,
					 Name = "P2",
					 Price = 50M
				 };

		// Arrange - create a new cart
		var target = new Cart();

		// Arrange - add some items
		target.AddItem (
				product : p1,
				quantity : 1
			);
		target.AddItem (
				product : p2,
				quantity : 1
			);

		// Act - reset the cart
		target.Clear();

		// Assert
		Assert.Empty (
				collection : target.Lines
			);
	}
}
