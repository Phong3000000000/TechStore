using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace equipment_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        public readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(int pg=1)
        {
            List<IdentityRole> role = await _dataContext.Roles.ToListAsync(); //33 datas


            //const int pageSize = 10; //10 items/trang

            //if (pg < 1) //page < 1;
            //{
            //    pg = 1; //page ==1
            //}
            //int recsCount = role.Count(); //33 items;

            //var pager = new Paginate(recsCount, pg, pageSize);

            //int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            ////category.Skip(20).Take(10).ToList()

            //var data = role.Skip(recSkip).Take(pager.PageSize).ToList();

            //ViewBag.Pager = pager;
            //return View(data);

            return View(role);

        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            var exists = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == role.Name);
            if (exists != null)
            {
                TempData["error"] = "Thêm thương hiệu thất bại - role đã tồn tại rồi";
                return RedirectToAction("Create");
            }
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                TempData["success"] = "Thêm role thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Thêm role thất bại";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                TempData["error"] = "Xóa role thất bại - không tồn tại Id của role này";
                return RedirectToAction("Index", "Role");
            }
            await _roleManager.DeleteAsync(role);
            TempData["success"] = "Xóa role thành công";
            return RedirectToAction("Index", "Role");
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                TempData["error"] = "Cập nhật role thất bại - không tồn tại Id của role này";
                return RedirectToAction("Index", "User");
            }
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var exists_role = await _roleManager.FindByIdAsync(role.Id);
                exists_role.Name = role.Name;


                IdentityResult result = await _roleManager.UpdateAsync(exists_role);
                if (result.Succeeded)
                {
                    TempData["success"] = "Cập nhật role thành công";
                    return RedirectToAction("Index", "Role");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            TempData["error"] = "Cập nhật role thất bại";
            return View(role);
        }
    }
}
