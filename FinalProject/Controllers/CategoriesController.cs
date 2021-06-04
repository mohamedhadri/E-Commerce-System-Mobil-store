using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Vm;
using FinalProject.Enum;
using Microsoft.AspNetCore.Http;
using FinalProject.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalProject.Areas.Identity.Data;

namespace FinalProject.Controllers
{
    public class CategoriesController : Controller
    {

        private FinalProjectContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CategoriesController(FinalProjectContext dbContext, IHostingEnvironment hostEnviornment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostEnviornment;
        }

        public IActionResult Index()
        {
            //get the category that not deleted
            var Categories = _dbContext.Categories.Where(x => x.IsDelete == false);
            //mapping to CategoryVm (which mean Put the data in VM)
            var CategoriesVm = Categories.Select(x => new CategoryVm()
            {
                Id = x.Id,
                Name= x.Name,
                CreationDate=x.CreateDate,
                ImageId = _dbContext.Attachments.FirstOrDefault(r => r.RecordId == x.Id.ToString()
                && r.RecordType == RecordType.Categories).FileName
            }) ;
            // display A variable (List) from VM in the Interface
            var list = _dbContext.Categories.Where(x => x.IsDelete == false).Select(x => new CategoryVm()
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreateDate,
                ImageId = _dbContext.Attachments.FirstOrDefault(r => r.RecordId == x.Id.ToString()
                && r.RecordType == RecordType.Categories).FileName
            });
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.ListParentCategories = _dbContext.Categories.Where
                (x => x.IsDelete == false).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryVm Category, List<IFormFile>files)
        {
         var entity=   await _dbContext.Categories.AddAsync(Category.ToEntity());
            await _dbContext.SaveChangesAsync();
            var CategoryDb = Category.ToEntity();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach(var file in files)
                    {
                        //generate random string for image id
                        var imageId = Guid.NewGuid();
                        //upload an image to Categories folder
                          var uploads = Path.Combine( _hostingEnvironment.WebRootPath, $"uploads/Categories/{imageId}.png");
                        await  using (var stream = System.IO.File.Create(uploads))
                          {
                           await file.CopyToAsync(stream);
                          }


                        var image = new Attachments()
                        {
                            RecordId=entity.Entity.Id.ToString(),
                            FileName=imageId.ToString(),
                            RecordType=RecordType.Categories
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                    return RedirectToAction("Index");

        }


        public async Task<IActionResult> EditAsync(int id)
        {
            //make conncetion with database and get categories
            var categoryDb = await _dbContext.Categories.FindAsync(id);
            //mapping to VM (convert entity category to categoryVm)
            var categoryVm = new CategoryVm()
            {
                Id = categoryDb.Id,
                Name = categoryDb.Name,
                Description=categoryDb.Description,
                ShortDescription= categoryDb.ShortDescription,
                ParentId=categoryDb.ParentId
            };
            //get list images for this category
            var listImages = _dbContext.Attachments
                .Where(x => x.RecordId == categoryDb.Id.ToString() && x.RecordType==RecordType.Categories)
                .Select(x => x.FileName);
            categoryVm.Images = listImages.ToList();

            ViewBag.ListParentCategories = _dbContext.Categories.Where
                (x => x.IsDelete == false)
                .Select(x => new SelectListItem() 
                { 
                    Text = x.Name, Value = x.Id.ToString()

                }).ToList();
            return View(categoryVm);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(CategoryVm Category, List<IFormFile> files)
        {
            //find category DB
            var entity = await _dbContext.Categories.FindAsync(Category.Id);

            //modify category
            entity.Name = Category.Name;
            entity.Description = Category.Description;
            entity.ShortDescription = Category.ShortDescription;
            entity.ParentId = Category.ParentId;

            await _dbContext.SaveChangesAsync();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        //generate random string for image id
                        var imageId = Guid.NewGuid();
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/Categories/{imageId}.png");
                        await using (var stream = System.IO.File.Create(uploads))
                        {
                            await file.CopyToAsync(stream);
                        }


                        var image = new Attachments()
                        {
                            RecordId = entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Categories
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }
                }

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteImage(string fileName)
        {
            var attachment = _dbContext.Attachments.FirstOrDefault(x => x.FileName == fileName);
            //get category id for this image
                var categoryId = Convert.ToInt32(attachment.RecordId);

               _dbContext.Attachments.Remove(attachment);

           await _dbContext.SaveChangesAsync();
            return RedirectToAction("Edit",new { id= categoryId});
        }


        public async Task<IActionResult> Delete(int id)
        {
            var brandDb = _dbContext.Categories.SingleOrDefault(x => x.Id == id);
            brandDb.IsDelete = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
