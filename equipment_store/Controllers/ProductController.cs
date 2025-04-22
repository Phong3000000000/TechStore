using equipment_store.Models;
using equipment_store.Models.ViewModels;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			var product=_dataContext.Producs.Include(x=>x.Rating).FirstOrDefault(x=>x.Id==Id);
			if (product==null)
			{
				return RedirectToAction("Index");
			}
			var relatedproducts = await _dataContext.Producs.Where(x => x.CategoryId == product.CategoryId && x.Id!= product.Id).Take(4).ToListAsync();
			ViewBag.relatedproducts=relatedproducts;

			var viewModel = new ProductDetailsViewModel
			{
				ProductDetails = product,
			};


			return View(viewModel);
		}

		public async Task<IActionResult>Search(string keyword)
		{
			var list = _dataContext.Producs.Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword)).ToList();
			if(list.Count==0)
			{
				TempData["error"] = "Không tồn tại sản phẩm cần tìm";
			}	
			ViewBag.keyword=keyword;
			return View(list);
		}

	
		public async Task<IActionResult> CommentProduct(RatingModel rating)
		{
			if(!ModelState.IsValid)
			{
				var ratingEntity = new RatingModel
				{
					ProductId = rating.ProductId,
					Name = rating.Name,
					Email = rating.Email,
					Comment = rating.Comment,
					Star = rating.Star,

				};
				await _dataContext.AddAsync(ratingEntity);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm đánh giá thành công";
				return Redirect(Request.Headers["Referer"]);		
			}
			TempData["error"] = "Thêm đánh giá thất bại";
			return Redirect(Request.Headers["Referer"]);

		}
	}
}
