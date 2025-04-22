using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class BrandController : Controller
    {
        public readonly DataContext _dataContext;
        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<BrandModel> brand = _dataContext.Brands.ToList(); //33 datas


   //         const int pageSize = 10; //10 items/trang

   //         if (pg < 1) //page < 1;
   //         {
   //             pg = 1; //page ==1
   //         }
   //         int recsCount = brand.Count(); //33 items;

   //         var pager = new Paginate(recsCount, pg, pageSize);

   //         int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

   //         //category.Skip(20).Take(10).ToList()

   //         var data = brand.Skip(recSkip).Take(pager.PageSize).ToList();

   //         ViewBag.Pager = pager;
			//return View(data);

			return View(brand);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            var exists = await _dataContext.Brands.FirstOrDefaultAsync(x=>x.Name==brand.Name);
            if (exists != null)
            {
                TempData["error"] = "Thêm thương hiệu thất bại - thương hiệu đã tồn tại rồi";
                return RedirectToAction("Create");
            }
            brand.slug = brand.Name.Replace(" ", "-");
            if (!ModelState.IsValid)
            {
                _dataContext.Brands.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm thương hiệu thất bại";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var brand = await _dataContext.Brands.FindAsync(Id);
            if (brand == null)
            {
                TempData["error"] = "Cập nhật thương hiệu thất bại - Thương hiệu không tồn tại";
                return RedirectToAction("Edit");
            }
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            if (!ModelState.IsValid)
            {
                brand.slug = brand.Name.Replace(" ", "-");
                _dataContext.Brands.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Cập nhật thương hiệu thất bại";
            return RedirectToAction("Edit");
        }


        public async Task<IActionResult> Remove(int Id)
        {
            var brand = await _dataContext.Brands.FindAsync(Id);
            if (brand == null)
            {
                TempData["error"] = "Xóa thương hiệu thất bại - thương hiệu không tồn tại";
                return RedirectToAction("Index");
            }
            return View(brand);

        }
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Remove_confirned(BrandModel brand)
        {
            if(!ModelState.IsValid)
            {
                _dataContext.Brands.Remove(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Xóa thương hiệu thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Xóa thương hiệu thất bại";
            return RedirectToAction("Remove");
        }



    }
}
