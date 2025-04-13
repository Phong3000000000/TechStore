using equipment_store.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace equipment_store.Repository
{
	public class DataContext:DbContext
	{
		public DataContext(DbContextOptions<DataContext> options):base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Producs { get; set; }
		public DbSet<CategoryModel>Categories { get; set; }

	}
}
