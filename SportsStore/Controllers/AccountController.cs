using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class AccountController : Controller
{
	private readonly SignInManager<IdentityUser> signInManager;
	private readonly UserManager<IdentityUser> userManager;

	public AccountController ( UserManager<IdentityUser> userMgr,
							   SignInManager<IdentityUser> signInMgr )
	{
		userManager = userMgr;
		signInManager = signInMgr;
	}

	public ViewResult Login ( string returnUrl ) => View (
			model : new LoginModel
					{
						Name = string.Empty,
						Password = string.Empty,
						ReturnUrl = returnUrl
					}
		);

	[ HttpPost ]
	[ ValidateAntiForgeryToken ]
	public async Task<IActionResult> Login ( LoginModel loginModel )
	{
		if ( ModelState.IsValid )
		{
			var user =
				await userManager.FindByNameAsync (
						userName : loginModel.Name
					);
			if ( user != null )
			{
				await signInManager.SignOutAsync();
				if ( ( await signInManager.PasswordSignInAsync (
							   user : user,
							   password : loginModel.Password,
							   isPersistent : false,
							   lockoutOnFailure : false
						   ) ).Succeeded )
				{
					return Redirect (
							url : loginModel?.ReturnUrl
							   ?? "/Admin"
						);
				}
			}

			ModelState.AddModelError (
					key : "",
					errorMessage : "Invalid name or password"
				);
		}

		return View (
				model : loginModel
			);
	}

	[ Authorize ]
	public async Task<RedirectResult> Logout ( string returnUrl = "/" )
	{
		await signInManager.SignOutAsync();
		return Redirect (
				url : returnUrl
			);
	}
}
