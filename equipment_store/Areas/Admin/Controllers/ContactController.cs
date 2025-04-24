using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace equipment_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(_dataContext.Contacts.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactModel contact)
        {
            if (!ModelState.IsValid)
            {
                if (contact.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/logo");
                    string imageName = Guid.NewGuid().ToString() + "_" + contact.ImageUpload.FileName;
                    string filepath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    await contact.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    contact.LogoImg = imageName;
                }
                _dataContext.Contacts.Add(contact);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Tạo footer thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có vài giá trị đang bị lổi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                }
                string errorMess = string.Join("\n", errors);
                return BadRequest(errorMess);
            }


        }


        public async Task<IActionResult> Edit(int Id)
        {
            ContactModel contact = await _dataContext.Contacts.FirstOrDefaultAsync(x=>x.Id==Id);
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactModel contact)
        {
            var existed_contact = _dataContext.Contacts.FirstOrDefault();


            if (!ModelState.IsValid)
            {
                if (contact.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/logo");
                    string imageName = Guid.NewGuid().ToString() + "_" + contact.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await contact.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_contact.LogoImg = imageName;
                }

                existed_contact.Name = contact.Name;
                existed_contact.Email = contact.Email;
                existed_contact.Description = contact.Description;
                existed_contact.Phone = contact.Phone;
                existed_contact.Map = contact.Map;


                _dataContext.Update(existed_contact);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thông tin web thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
           
        }

        public async Task<IActionResult> Remove(int Id)
        {
            var contact = await _dataContext.Contacts.FindAsync(Id);
            if (contact == null)
            {
                TempData["error"] = "Xóa footer thất bại - không tồn tại Id của footer này";
                return RedirectToAction("Index");
            }
            _dataContext.Contacts.Remove(contact);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa footer thành công";
            return RedirectToAction("Index");
        }
    }
}
