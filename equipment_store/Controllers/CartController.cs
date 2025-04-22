using equipment_store.Models;
using equipment_store.Models.ViewModels;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace equipment_store.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _datacontext;
		public CartController(DataContext context)
		{
			_datacontext = context;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GranTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product=await _datacontext.Producs.FindAsync(Id);
			if (product == null) return RedirectToAction("Index");
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItem=cart.FirstOrDefault(x=>x.ProductId==Id);
			if (cartItem != null)
			{
				cartItem.Quantity++;
			}
			else
			{
				cart.Add(new CartItemModel(product));
			}
			HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Add product to cart is successfully";
            return Redirect(Request.Headers["Referer"].ToString());

		}

		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem=cartItems.FirstOrDefault(x=>x.ProductId== Id);
			if (cartItem.Quantity>1)
			{
				cartItem.Quantity--;
			}
			else
			{
				cartItems.Remove(cartItem);
			}
			if (cartItems.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cartItems);
			}
            TempData["success"] = "Decrease quantity is successfully";
            return RedirectToAction("Index");

		}

		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem=cartItems.FirstOrDefault(x=>x.ProductId == Id);
			if(cartItem.Quantity>0)
			{
				cartItem.Quantity++;
			}	
			if(cartItems.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cartItems);
			}
            TempData["success"] = "Increase quantity is successfully";
            return RedirectToAction("index");
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem=cartItems.FirstOrDefault(x=>x.ProductId==Id);
			if(cartItem!=null)
			{
				cartItems.Remove(cartItem);
			}
			HttpContext.Session.SetJson("Cart", cartItems);
            TempData["success"] = "Remove product is successfully";
            return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			cartItems.Clear();
			HttpContext.Session.SetJson("Cart", cartItems);
            TempData["success"] = "Clear cart is successfully";
            return RedirectToAction("index");
		}
		public IActionResult Checkout()
		{
			return View();
		}

		
	}
}
