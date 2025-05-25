using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class NavigationMenuViewComponent : ViewComponent
{
	private readonly IStoreRepository repository;

	public NavigationMenuViewComponent ( IStoreRepository repo ) { repository = repo; }

	public IViewComponentResult Invoke()
	{
		ViewBag.SelectedCategory = RouteData?.Values[key : "category"];
		return View (
				model : repository.Products.Select (
							selector : x => x.Category
						).
					Distinct().
					OrderBy (
							keySelector : x => x
						)
			);
	}
}
