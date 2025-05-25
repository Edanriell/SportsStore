namespace SportsStore.Models;

public class EFStoreRepository : IStoreRepository
{
	private readonly StoreDbContext context;

	public EFStoreRepository ( StoreDbContext ctx ) { context = ctx; }

	public IQueryable<Product> Products => context.Products;

	public void CreateProduct ( Product p )
	{
		context.Add (
				entity : p
			);
		context.SaveChanges();
	}

	public void DeleteProduct ( Product p )
	{
		context.Remove (
				entity : p
			);
		context.SaveChanges();
	}

	public void SaveProduct ( Product p ) { context.SaveChanges(); }
}
