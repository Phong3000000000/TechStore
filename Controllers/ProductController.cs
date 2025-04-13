using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;

namespace equipment_store.Controllers
{
	public class ProductController : Controller
	{
		DataContext _dataContext;
		public ProductController(DataContext context)
		{
			_dataContext = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult>Details(int Id)
		{
			var product=_dataContext.Producs.FirstOrDefault(x=>x.Id==Id);
			if (product==null)
			{
				return RedirectToAction("Index");
			}

			return View(product);
		}
	}
}
