using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Models;

namespace SportsStore.Pages;

public class CartModel : PageModel
{
	private IStoreRepository repository;

	public CartModel ( IStoreRepository repo, Cart cartService )
	{
		repository = repo;
		Cart = cartService;
	}

	public Cart Cart { get; set; }
	public string ReturnUrl { get; set; } = "/";

	public void OnGet ( string returnUrl ) { ReturnUrl = returnUrl ?? "/"; }

	public IActionResult OnPost ( long productId, string returnUrl )
	{
		var product = repository.Products.FirstOrDefault (
				predicate : p => p.ProductID == productId
			);
		if ( product != null )
		{
			Cart.AddItem (
					product : product,
					quantity : 1
				);
		}

		return RedirectToPage (
				routeValues : new
							  {
								  returnUrl
							  }
			);
	}

	public IActionResult OnPostRemove ( long productId,
										string returnUrl )
	{
		Cart.RemoveLine (
				product : Cart.Lines.First (
							predicate : cl =>
								cl.Product.ProductID == productId
						).
					Product
			);
		return RedirectToPage (
				routeValues : new
							  {
								  returnUrl
							  }
			);
	}
}
