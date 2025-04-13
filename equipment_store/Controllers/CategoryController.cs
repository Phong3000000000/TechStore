using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace equipment_store.Controllers
{
	public class CategoryController:Controller
	{
		DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(string Slug)
		{
			var category = _dataContext.Categories.FirstOrDefault(c=>c.slug==Slug);
			if (category == null)
			{
				return RedirectToAction("index");
			}
			var product = _dataContext.Producs.Where(p => p.CategoryId == category.Id);
			return View(await product.OrderByDescending(x => x.Id).ToListAsync());
		}
	}
}
