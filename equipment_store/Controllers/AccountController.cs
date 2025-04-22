using equipment_store.Models;
using equipment_store.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace equipment_store.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;

		public AccountController(UserManager<AppUserModel> UserManager, SignInManager<AppUserModel> SignInManager) 
		{
			_signInManager = SignInManager;
			_userManager = UserManager;
		}


		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl=returnUrl});
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if(!ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password,false,false);
				if(result.Succeeded)
				{
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "Username hoặc password bị sai");
			}	
			return View(loginVM);
		}

		public async Task<IActionResult> Signup()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Signup(UserModel user)
		{
            if (ModelState.IsValid)
            {
				AppUserModel newuser = new AppUserModel { UserName = user.Username,Email=user.Email};
				IdentityResult result=await _userManager.CreateAsync(newuser, user.Password);
				if (result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
					return RedirectToAction("Signup");
				}
				foreach(IdentityError error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
					
            }
			TempData["error"] = "Tạo user thất bại";
			return View(user);
		}


		public async Task<IActionResult>Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
