using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace equipment_store.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles ="Admin, Publisher")]
	public class SliderController : Controller
	{
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
		{
			
			return View(await _dataContext.Slider.ToListAsync() );
		}

		public IActionResult Create()
		{
			return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderModel slider)
        {
            if (!ModelState.IsValid)
            {
                    if (slider.ImageUpload != null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                        string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                        string filepath = Path.Combine(uploadDir, imageName);

                        FileStream fs = new FileStream(filepath, FileMode.Create);
                        await slider.ImageUpload.CopyToAsync(fs);
                        fs.Close();

                    slider.Imgae = imageName;
                    }
                    _dataContext.Slider.Add(slider);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Tạo slider thành công";
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


        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _dataContext.Slider.FindAsync(id);
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderModel slider)
        {

            if (!ModelState.IsValid)
            {
                var old_slider = _dataContext.Slider.Find(slider.Id);
                if (slider.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                    string filepath = Path.Combine(uploadDir, imageName);


                    string oldfilepath = Path.Combine(uploadDir, old_slider.Imgae);
                    if (System.IO.File.Exists(oldfilepath))
                    {
                        System.IO.File.Delete(oldfilepath);
                    }

                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    await slider.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    old_slider.Imgae = imageName;

                }
                old_slider.Name = slider.Name;
                old_slider.Description = slider.Description;
                old_slider.Status = slider.Status;



                _dataContext.Slider.Update(old_slider);
                await _dataContext.SaveChangesAsync();
                //_dataContext.SaveChanges();
                TempData["success"] = "Cập nhật slider thành công";
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

        public async Task<IActionResult> Remove(int Id)
        {
            var slider = await _dataContext.Slider.FindAsync(Id);
            if (slider == null)
            {
                TempData["error"] = "Xóa slider thất bại - không tồn tại Id của slider này";
                return RedirectToAction("Index");
            }
            _dataContext.Slider.Remove(slider);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa slider thành công";
            return RedirectToAction("Index");
        }
    }
}
