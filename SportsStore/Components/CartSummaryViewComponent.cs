﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class CartSummaryViewComponent : ViewComponent
{
	private readonly Cart cart;

	public CartSummaryViewComponent ( Cart cartService ) { cart = cartService; }

	public IViewComponentResult Invoke() => View (
			model : cart
		);
}
