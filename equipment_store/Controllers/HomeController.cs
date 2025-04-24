using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

namespace equipment_store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _datacontext;
        private readonly UserManager<AppUserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, DataContext context, UserManager<AppUserModel> userManager)
        {
            _logger = logger;
            _datacontext = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var products=_datacontext.Producs.ToList();

            var sliders=_datacontext.Slider.Where(x=>x.Status=="1").ToList();
            ViewBag.Sliders=sliders;

            return View(products);
        }

        public IActionResult Contact()
        {
            return View();
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

        public async Task<IActionResult> Compare()
        {
            var listcompare = await (from c in _datacontext.Compares
                                       join u in _datacontext.Users on c.UserId equals u.Id
                                       join p in _datacontext.Producs on c.ProductId equals p.Id
                                       select new { User = u, Compare = c, Product = p }
                                     ).ToListAsync();
            return View(listcompare);
        }

        public async Task<IActionResult> DeleteCompare(int Id)
        {
            var compare = await _datacontext.Compares.FindAsync(Id);
            if(compare==null)
            {
                TempData["error"] = "Xóa so sánh thất bại - không tôn tại Id của so sánh cần xóa";
                return RedirectToAction("Compare", "Home");
            }    
            _datacontext.Compares.Remove(compare);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Xóa so sánh thành công";
            return RedirectToAction("Compare", "Home");

        }

        public async Task<IActionResult> DeleteWishlist(int Id)
        {
            var wishlist = await _datacontext.Wisthlists.FindAsync(Id);
            if (wishlist == null)
            {
                TempData["error"] = "Xóa sản phẩm yêu thích thất bại - không tôn tại Id của sản phẩm yêu thích cần xóa";
                return RedirectToAction("Compare", "Home");
            }
            _datacontext.Wisthlists.Remove(wishlist);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Xóa sản phẩm yêu thích thành công";
            return RedirectToAction("Wishlist", "Home");

        }



        public async Task<IActionResult> Wishlist()
        {
            var wishlist = await (from w in _datacontext.Wisthlists
                                  join u in _datacontext.Users on w.UserId equals u.Id
                                  join p in _datacontext.Producs on w.ProductId equals p.Id
                                  select new { Wishlist = w, User = u, Product = p }).ToListAsync();
            return View(wishlist);
        }


        [HttpPost]
        public async Task<IActionResult> AddWishlist(int Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var wishlistProduct = new WisthlistModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            _datacontext.Wisthlists.Add(wishlistProduct);
            try
            {
                await _datacontext.SaveChangesAsync();
                return Ok(new { success = true, message = "Add to wishlisht Successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while adding to wishlist table.");
            }

        }
		[HttpPost]
		public async Task<IActionResult> AddCompare(int Id)
		{
			var user = await _userManager.GetUserAsync(User);

			var comapre = new CompareModel
			{
				ProductId = Id,
				UserId = user.Id
			};

			_datacontext.Compares.Add(comapre);
			try
			{
				await _datacontext.SaveChangesAsync();
				return Ok(new { success = true, message = "Add to compare Successfully" });
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while adding to wishlist table.");
			}

		}
	}
}
