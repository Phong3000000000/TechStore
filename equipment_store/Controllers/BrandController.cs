using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext context)
		{
			_dataContext = context;

		}
		public async Task<IActionResult> Index(string Slug)
		{
			var brand=_dataContext.Brands.FirstOrDefault(b=>b.slug==Slug);
			if (brand==null) return RedirectToAction("Index");
			var product = _dataContext.Producs.Where(p => p.BrandId == brand.Id);
			return View(await product.OrderByDescending(p=>p.Price).ToListAsync());
		}
	}
}
