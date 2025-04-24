using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace equipment_store.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Publisher")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            List<ProductModel> product = await _dataContext.Producs.Include(x => x.Category).Include(x => x.Brand).ToListAsync(); //33 datas


            //const int pageSize = 10; //10 items/trang

            //if (pg < 1) //page < 1;
            //{
            //    pg = 1; //page ==1
            //}
            //int recsCount = product.Count(); //33 items;

            //var pager = new Paginate(recsCount, pg, pageSize);

            //int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            ////category.Skip(20).Take(10).ToList()

            //var data = product.Skip(recSkip).Take(pager.PageSize).ToList();

            //ViewBag.Pager = pager;
            //return View(data);
            return View(product); 
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                product.slug = product.Name.Replace(" ", "-");
                var exists_product = await _dataContext.Producs.FirstOrDefaultAsync(x => x.slug == product.slug);
                //var exists_product =  _dataContext.Producs.FirstOrDefault(x => x.slug == product.slug);
                if (exists_product!=null)
                {
                    TempData["error"] = "Sản phầm này đã có trong cửa hàng rồi";
                    return RedirectToAction("Create");
                }    
                else
                {
                    if(product.ImageUpload!=null)
                    {
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        string imageName=Guid.NewGuid().ToString()+"_"+product.ImageUpload.FileName;
                        string filepath=Path.Combine(uploadDir, imageName);

                        FileStream fs=new FileStream(filepath, FileMode.Create);
                        await product.ImageUpload.CopyToAsync(fs);
                        fs.Close();

                        product.Image = imageName;
                    }
                    _dataContext.Producs.Add(product);
                    await _dataContext.SaveChangesAsync();
                    //_dataContext.SaveChanges();
                    TempData["success"] = "Tạo sản phẩm thành công";
                    return RedirectToAction("Index");
                }    

            }
            else
            {
                TempData["error"] = "Model có vài giá trị đang bị lổi";
                List<string> errors=new List<string>();
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
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
            var product = await _dataContext.Producs.FindAsync(id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {

            if (!ModelState.IsValid)
            {
                product.slug = product.Name.Replace(" ", "-");
                var exists_product = _dataContext.Producs.Where(x => x.Name == product.Name);
                var old_product = _dataContext.Producs.Find(product.Id);
                //var exists_product = _dataContext.Producs.FirstOrDefault(x => x.slug == product.slug);
                if (exists_product.Count()>1)
                {
                    product.slug = product.slug + "-" + product.Id;

                }
                if (product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filepath = Path.Combine(uploadDir, imageName);


                    string oldfilepath=Path.Combine(uploadDir, old_product.Image);
                    if(System.IO.File.Exists(oldfilepath))
                    {
                        System.IO.File.Delete(oldfilepath);
                    }    

                    FileStream fs = new FileStream(filepath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    old_product.Image = imageName;
      
                }
                old_product.Name= product.Name;
                old_product.CategoryId  = product.CategoryId;
                old_product.BrandId = product.BrandId;
                old_product.Description = product.Description;
                old_product.Price = product.Price;



                _dataContext.Producs.Update(old_product);
                await _dataContext.SaveChangesAsync();
                //_dataContext.SaveChanges();
                TempData["success"] = "Cập nhật sản phẩm thành công";
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
            var product = await _dataContext.Producs.FindAsync(Id);
            if(product==null)
            {
                TempData["Error"] = "Không tồn tại sản phẩm cần xóa";
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove_confirned(ProductModel product)
        {
            var exitst_product = await _dataContext.Producs.FindAsync(product.Id);
            if(!string.Equals(exitst_product.Image, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldfilepath = Path.Combine(uploadDir, exitst_product.Image);
                if(System.IO.File.Exists(oldfilepath))
                {
                    System.IO.File.Delete(oldfilepath);
                }    

            }



            _dataContext.Producs.Remove(exitst_product);
            await _dataContext.SaveChangesAsync();
            TempData["Success"] = "Xóa sản phẩm thành công";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> AddQuantity(int Id)
        {
            var product = await _dataContext.ProductQuantities.Where(x => x.ProductId == Id).ToListAsync();
            ViewBag.Id = Id;

            ViewBag.ProductQuantity= product;
            ViewBag.Sum = product.Sum(x => x.Quantity);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuantity_Post(ProductQuantityModel productquantity)
        {
            var product = await _dataContext.Producs.FindAsync(productquantity.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            product.Quantity += productquantity.Quantity;


            _dataContext.ProductQuantities.Add(productquantity);
            await _dataContext.SaveChangesAsync();
            TempData["Success"] = "Thêm số lượng thành công";
            return RedirectToAction("AddQuantity", "Product", new {Id= productquantity.ProductId});
        }




    }
}
