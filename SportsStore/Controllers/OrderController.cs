using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
	private readonly Cart cart;
	private readonly IOrderRepository repository;

	public OrderController ( IOrderRepository repoService,
							 Cart cartService )
	{
		repository = repoService;
		cart = cartService;
	}

	public ViewResult Checkout() => View (
			model : new Order()
		);

	[ HttpPost ]
	public IActionResult Checkout ( Order order )
	{
		if ( cart.Lines.Count() == 0 )
		{
			ModelState.AddModelError (
					key : "",
					errorMessage : "Sorry, your cart is empty!"
				);
		}

		if ( ModelState.IsValid )
		{
			order.Lines = cart.Lines.ToArray();
			repository.SaveOrder (
					order : order
				);
			cart.Clear();
			return RedirectToPage (
					pageName : "/Completed",
					routeValues : new
								  {
									  orderId = order.OrderID
								  }
				);
		}

		return View();
	}
}
