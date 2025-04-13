using Microsoft.AspNetCore.Mvc;

namespace equipment_store.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
