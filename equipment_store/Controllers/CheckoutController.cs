using equipment_store.Areas.Admin.Repository;
using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace equipment_store.Controllers
{
	public class CheckoutController:Controller
	{
		private readonly DataContext _dataContext;
		private readonly IEmailSender _emailSender;
		public CheckoutController(DataContext context,IEmailSender emailSender)
		{
			_dataContext = context;
			_emailSender = emailSender;
		}

		public async Task<IActionResult>Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail==null)
			{
				return RedirectToAction("Login", "Account");
			}	
			else
			{
				var ordercode=Guid.NewGuid().ToString();
				var order = new OrderModel();
				order.UserName = userEmail;
				order.OrderCode = ordercode;
				order.CreatedDate = DateTime.Now;
				order.Status = 1;
				_dataContext.Add(order);
				_dataContext.SaveChanges();


				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

				foreach (var item in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.Price = item.Price;
					orderdetails.Quantity = item.Quantity;
					orderdetails.ProductId = (int)item.ProductId;
					_dataContext.OrderDetails.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");

				var receiver = userEmail;
				var subject = "Thông báo thanh toán";
				var message = "Chúc mừng! Đơn hàng được cài đặt thành công và sẽ giao đến bạn trong thời gian sớm nhất";

				await _emailSender.SendEmailAsync(receiver, subject, message);	

				TempData["success"] = "Checkout thành công, vui lòng chờ duyệt đơn hàng";
				return RedirectToAction("Index", "Cart");
			}	

		}
	}
}
