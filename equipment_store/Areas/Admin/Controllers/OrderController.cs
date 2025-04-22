using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController:Controller
	{
		private readonly DataContext _dataContext;
		public OrderController(DataContext context)
		{
			_dataContext = context;
		}
		public async Task<IActionResult> Index(int pg = 1)
		{
            List<OrderModel> order = _dataContext.Orders.ToList(); //33 datas


   //         const int pageSize = 10; //10 items/trang

   //         if (pg < 1) //page < 1;
   //         {
   //             pg = 1; //page ==1
   //         }
   //         int recsCount = order.Count(); //33 items;

   //         var pager = new Paginate(recsCount, pg, pageSize);

   //         int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

   //         //category.Skip(20).Take(10).ToList()

   //         var data = order.Skip(recSkip).Take(pager.PageSize).ToList();

   //         ViewBag.Pager = pager;
			//return View(data);

            return View(order);
        }

		public async Task<IActionResult> ViewOrder(string Ordercode)
		{
			var orderdetails = await _dataContext.OrderDetails.Include(p => p.Product).Where(od => od.OrderCode == Ordercode).ToListAsync();
			var order = _dataContext.Orders.FirstOrDefault(x => x.OrderCode == Ordercode);
			ViewBag.status = order.Status;
			return View(orderdetails);
		}

        public async Task<IActionResult> Remove(string Ordercode)
        {
			var order = await _dataContext.Orders.FirstOrDefaultAsync(x=>x.OrderCode==Ordercode);
			if(order==null)
			{
				TempData["error"] = "Xóa đơn hàng thất bại - đơn hàng không tồn tại";
				return RedirectToAction("Index", "Order");
			}	
			_dataContext.Orders.Remove(order);
			foreach(var item in _dataContext.OrderDetails)
			{
				if(item.OrderCode==Ordercode)
				{
                    _dataContext.OrderDetails.Remove(item);
					

                }	
			}
            _dataContext.SaveChanges();
            TempData["success"] = "Xóa đơn hàng thành công";
            return RedirectToAction("Index", "Order");
        }

		[HttpPost]
		public async Task<IActionResult> UpdateOrder(string ordercode, int status)
		{
			var order=await _dataContext.Orders.FirstOrDefaultAsync(x=>x.OrderCode == ordercode);
			if (order==null)
			{
				return NotFound();
			}	
			order.Status = status;

			await _dataContext.SaveChangesAsync();
			return Ok(new { success = true});
		}

    }
}
