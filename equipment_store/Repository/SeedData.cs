using equipment_store.Models;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Repository
{
	public class SeedData
	{
		public static void  SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if(!_context.Producs.Any())
			{
				CategoryModel Macboook = new CategoryModel() { Name= "Macboook" , Description= "Macboook is the largest prodcut int the world", slug="macbook", Status="1" };
				CategoryModel Pc = new CategoryModel() { Name = "Pc", Description = "Pc is the largest prodcut int the world", slug = "Pc", Status = "1" };
				BrandModel apple=new BrandModel() { Name="Apple", Description="Apple is the best", slug="Apple", Status="1"};
				BrandModel samsung = new BrandModel() { Name = "Samsung", Description = "Samsung is the second", slug = "Samsung", Status = "1" };
				_context.Producs.AddRange(
					new ProductModel() { Name="MacBook", slug="MacBook", Description="MacBook is the best", Image="1.jpg", Price=1200, Category=Macboook, Brand=apple},
					new ProductModel() { Name = "Pc", slug = "Pc", Description = "Pc is the best", Image = "2.jpg", Price = 1100 , Category=Pc, Brand=samsung}


					);
				_context.SaveChanges();
			}	

		}
	}
}
