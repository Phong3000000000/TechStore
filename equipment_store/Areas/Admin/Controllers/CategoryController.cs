using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext context)
		{
			_dataContext = context;
		}
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<CategoryModel> category = _dataContext.Categories.ToList(); //33 datas


   //         const int pageSize = 10; //10 items/trang

   //         if (pg < 1) //page < 1;
   //         {
   //             pg = 1; //page ==1
   //         }
   //         int recsCount = category.Count(); //33 items;

   //         var pager = new Paginate(recsCount, pg, pageSize);

   //         int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

   //         //category.Skip(20).Take(10).ToList()

   //         var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

   //         ViewBag.Pager = pager;
			//return View(data);

			return View(category);
        }
        public async Task<IActionResult> Create()
        {
			return View();
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
			if(!ModelState.IsValid)
			{
				var exists = await _dataContext.Categories.FirstOrDefaultAsync(x=>x.Name==category.Name);
				category.slug = category.Name.Replace(" ", "-");
				if (exists != null)
				{
					TempData["error"] = "Thêm danh mục thất bại - danh mục đã tồn tại";
					return RedirectToAction("Create");
				}
				_dataContext.Categories.Add(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Thêm danh mục thành công";
				return RedirectToAction("Index");
			}
            TempData["error"] = "Thêm danh mục thất bại";
            return RedirectToAction("Index");
        }



		public async Task<IActionResult> Remove(int Id)
		{
			var exists = await _dataContext.Categories.FindAsync(Id);
			if (exists == null)
			{
				TempData["Error"] = "Xóa thất bại - danh mục không tồn tại";
				return RedirectToAction("Remove");
			}
			return View(exists);

		}
		[HttpPost, ActionName("Remove")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove_confirned(CategoryModel category)
        {
            _dataContext.Categories.Remove(category);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Xóa danh mục thành công";
			return RedirectToAction("Index");

        }


		public async Task<IActionResult> Edit(int Id)
		{
			var category = await _dataContext.Categories.FindAsync(Id);
			if(category==null)
			{
				TempData["error"] = "Cập nhật danh mục thất bại - danh mục không tồn tại";
				return RedirectToAction("Edit");
			}	
			return View(category);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryModel category)
		{
			if(!ModelState.IsValid)
			{
				category.slug = category.Name.Replace(" ", "-");
				_dataContext.Update(category);
				await _dataContext.SaveChangesAsync();
				TempData["success"] = "Cập nhật danh mục thành công";
				return RedirectToAction("Index");
			}
			TempData["error"] = "Cập nhật danh mục thất bại";
			return RedirectToAction("Edit");	
		}

    }
}

