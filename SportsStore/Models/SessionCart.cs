using System.Text.Json.Serialization;
using SportsStore.Infrastructure;

namespace SportsStore.Models;

public class SessionCart : Cart
{
	[ JsonIgnore ]
	public ISession? Session { get; set; }

	public static Cart GetCart ( IServiceProvider services )
	{
		var session =
			services.GetRequiredService<IHttpContextAccessor>().
				HttpContext?.Session;
		var cart = session?.GetJson<SessionCart> (
						   key : "Cart"
					   )
				?? new SessionCart();
		cart.Session = session;
		return cart;
	}

	public override void AddItem ( Product product, int quantity )
	{
		base.AddItem (
				product : product,
				quantity : quantity
			);
		Session?.SetJson (
				key : "Cart",
				value : this
			);
	}

	public override void RemoveLine ( Product product )
	{
		base.RemoveLine (
				product : product
			);
		Session?.SetJson (
				key : "Cart",
				value : this
			);
	}

	public override void Clear()
	{
		base.Clear();
		Session?.Remove (
				key : "Cart"
			);
	}
}
