using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace equipment_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dataContext = context;
        }

        public async Task<IActionResult>Index( int pg=1)
        {
            var user_role = await (from u in _dataContext.Users
                                   join ur in _dataContext.UserRoles on u.Id equals ur.UserId
                                   join r in _dataContext.Roles on ur.RoleId equals r.Id
                                   select new { user = u, rolename=r.Name }).ToListAsync();


            //const int pageSize = 10; //10 items/trang

            //if (pg < 1) //page < 1;
            //{
            //    pg = 1; //page ==1
            //}
            //int recsCount = user_role.Count(); //33 items;

            //var pager = new Paginate(recsCount, pg, pageSize);

            //int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            ////category.Skip(20).Take(10).ToList()

            //var data = user_role.Skip(recSkip).Take(pager.PageSize).ToList();

            //ViewBag.Pager = pager;
            //return View(data);

            return View(user_role);
        }


        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {          
                IdentityResult result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {

                    var role=await _roleManager.FindByIdAsync(user.RoleId);
                    await _userManager.AddToRoleAsync(user, role.Name);
                    TempData["success"] = "Tạo user thành công";
                    return RedirectToAction("Index", "User");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", user.RoleId);
            TempData["error"] = "Tạo user thất bại";
            return View(user);
        }


        public async Task<IActionResult>Remove(string Id)
        {
            var user=await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["error"] = "Xóa user thất bại - không tồn tại Id của user này";
                return RedirectToAction("Index", "User");
            }
            await _userManager.DeleteAsync(user);
            TempData["success"] = "Xóa user thành công";
            return RedirectToAction("Index", "User");
        }



        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["error"] = "Cập nhật user thất bại - không tồn tại Id của user này";
                return RedirectToAction("Index", "User");
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", user.RoleId);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var exists_user =await _userManager.FindByIdAsync(user.Id);
                exists_user.UserName = user.UserName;
                exists_user.Email= user.Email;
                exists_user.PasswordHash = user.PasswordHash;
                exists_user.PhoneNumber = user.PhoneNumber;
                exists_user.RoleId = user.RoleId;

                var role_old = await _userManager.GetRolesAsync(exists_user);
                await _userManager.RemoveFromRolesAsync(exists_user, role_old);

                var role_new = await _roleManager.FindByIdAsync(exists_user.RoleId);
                await _userManager.AddToRoleAsync(exists_user, role_new.Name);

        

                IdentityResult result = await _userManager.UpdateAsync(exists_user);
                if (result.Succeeded)
                {
                    TempData["success"] = "Cập nhật user thành công";
                    return RedirectToAction("Index", "User");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name", user.RoleId);
            TempData["error"] = "Cập nhật user thất bại";
            return View(user);
        }

    }
}
