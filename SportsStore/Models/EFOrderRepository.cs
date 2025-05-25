using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public class EFOrderRepository : IOrderRepository
{
	private readonly StoreDbContext context;

	public EFOrderRepository ( StoreDbContext ctx ) { context = ctx; }

	public IQueryable<Order> Orders => context.Orders.Include (
				navigationPropertyPath : o => o.Lines
			).
		ThenInclude (
				navigationPropertyPath : l => l.Product
			);

	public void SaveOrder ( Order order )
	{
		context.AttachRange (
				entities : order.Lines.Select (
						selector : l => l.Product
					)
			);
		if ( order.OrderID == 0 )
		{
			context.Orders.Add (
					entity : order
				);
		}

		context.SaveChanges();
	}
}
