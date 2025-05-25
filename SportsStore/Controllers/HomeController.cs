using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class HomeController : Controller
{
	private readonly IStoreRepository repository;
	public int PageSize = 4;

	public HomeController ( IStoreRepository repo ) { repository = repo; }

	public ViewResult Index ( string? category, int productPage = 1 ) => View (
			model : new ProductsListViewModel
					{
						Products = repository.Products.Where (
									predicate : p => category == null
												  || p.Category == category
								).
							OrderBy (
									keySelector : p => p.ProductID
								).
							Skip (
									count : ( productPage - 1 ) * PageSize
								).
							Take (
									count : PageSize
								),
						PagingInfo = new PagingInfo
									 {
										 CurrentPage = productPage,
										 ItemsPerPage = PageSize,
										 TotalItems = category == null
														  ? repository.Products.Count()
														  : repository.Products.Where (
																	  predicate : e =>
																		  e.Category == category
																  ).
															  Count()
									 },
						CurrentCategory = category
					}
		);
}
