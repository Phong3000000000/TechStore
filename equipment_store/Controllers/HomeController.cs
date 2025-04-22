using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace equipment_store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _datacontext;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            _datacontext = context;
        }

        public IActionResult Index()
        {
            var products=_datacontext.Producs.ToList();

            var sliders=_datacontext.Slider.Where(x=>x.Status=="1").ToList();
            ViewBag.Sliders=sliders;

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if(statuscode==404)
            {
                return View("NotFound");
            }   
            else
            {
				return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}    

        }
    }
}
