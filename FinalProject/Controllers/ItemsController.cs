using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Enum;
using FinalProject.Vm;
using FinalProject.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Controllers
{
    public class ItemsController : Controller
    {
        public FinalProjectContext _dbContext { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;

        public ItemsController(FinalProjectContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
           var items = _dbContext.Items.FirstOrDefault(x => x.IsDelete == false).ToList.Select(x => new ItemsVm()
           {
              Id= x.Id,
              Name= x.Name,
              Price=x.Price,
              ImageId=_dbContext.Attachments.FirstOrDefault(r=> r.RecordId== x.Id.ToString()&& r.RecordType==RecordType.Items)?.FileName

           });
          

            return View();
        }


        public IActionResult Create()
        {
            ViewBag.ListCategories = _dbContext.Categories.Where(x => x.IsDelete == false)
                .Select(x=> new SelectListItem() 
                {
                    Text=x.Name,
                    Value=x.Id.ToString()
                }).ToList();

            ViewBag.Brands = _dbContext.Brands.Where(x => x.IsDelete == false).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ItemsVm item, List<IFormFile> files)
        {
            var entity = await _dbContext.Items.AddAsync(item.ToEntity());
            await _dbContext.SaveChangesAsync();
            var CategoryDb = item.ToEntity();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        //generate random string for image id
                        var imageId = Guid.NewGuid();
                        //upload an image to Categories folder
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/Items/{imageId}.png");
                        await using (var stream = System.IO.File.Create(uploads))
                        {
                            await file.CopyToAsync(stream);
                        }


                        var image = new Attachments()
                        {
                            RecordId = entity.Entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Items
                        };
                        await _dbContext.Attachments.AddAsync(image);
                        await _dbContext.SaveChangesAsync();
                    }
                }

            return RedirectToAction("Index");

        }


       public async Task<IActionResult> Edit(int id)
        {
            var itemDb = await _dbContext.Items.FindAsync(id);
            var itemVm = new ItemsVm()
            {
                Id=itemDb.Id,
                BrandId=itemDb.BrandId,
                CategoryId= itemDb.CategoryId,
                Color= itemDb.Color,
                ShortDescription= itemDb.ShortDescription,
                Description= itemDb.Description,
                Name= itemDb.Name,
                Price= itemDb.Price,
                Images=_dbContext.Attachments.Where(r=> r.RecordId==itemDb.Id.ToString()&& r.RecordType== RecordType.Items).Select(r=> r.FileName).ToList()


            };

            ViewBag.ListCategories = _dbContext.Categories.Where(x => x.IsDelete == false)
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            ViewBag.Brands = _dbContext.Brands.Where(x => x.IsDelete == false).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            return View(itemVm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ItemsVm item, List<IFormFile> files)
        {
            var itemDb =  await _dbContext.Items.FindAsync(item.Id);
            itemDb.BrandId = item.BrandId;
            itemDb.CategoryId = item.CategoryId;
            itemDb.Color = item.Color;
            itemDb.Description = item.Description;
            itemDb.Name = item.Name;
            itemDb.ShortDescription = item.ShortDescription; 

            var entity =  _dbContext.Items.Update(itemDb);
            await _dbContext.SaveChangesAsync();
            var CategoryDb = item.ToEntity();
            if (files != null)
                if (files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        //generate random string for image id
                        var imageId = Guid.NewGuid();
                        //upload an image to Categories folder
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/Items/{imageId}.png");
                        await using (var stream = System.IO.File.Create(uploads))
                        {
                            await file.CopyToAsync(stream);
                        }


                        var image = new Attachments()
                        {
                            RecordId = entity.Entity.Id.ToString(),
                            FileName = imageId.ToString(),
                            RecordType = RecordType.Items
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
            return RedirectToAction("Edit", new { id = categoryId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var itemDb = _dbContext.Items.SingleOrDefault(x => x.Id == id);
            itemDb.IsDelete = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

    }
}
