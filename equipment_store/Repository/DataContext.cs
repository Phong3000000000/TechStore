using equipment_store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace equipment_store.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<SliderModel> Slider {  get; set; }
		public DbSet<ProductModel> Producs { get; set; }
		public DbSet<CategoryModel>Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<RatingModel> Ratings { get; set; }

	}
}
