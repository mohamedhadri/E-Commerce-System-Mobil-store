using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using FinalProject.Vm;
using FinalProject.Areas.Identity.Data;

namespace WebApplication1.Controllers
{
    public class BrandsController : Controller
    {
        public FinalProjectContext _dbContext { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;

        public BrandsController(FinalProjectContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            // connection to the database
            var listBrands = _dbContext.Brands.Where(brand => brand.IsDelete == false) .Select(brand => new BrandsVm()
                {
                    Name = brand.Name,
                    Id = brand.Id,
                    CreationDate = brand.CreateDate,
                    ImageId= brand.ImageId+ ".png"
                }
               ).ToList();
            return View(listBrands);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandsVm brand, IFormFile file)
        {

            var newBrand = new Brands() { Name = brand.Name, Description = brand.Description };
            if(file!=null)
            if (file.Length > 0)
            {
                    //generate random string for image id
                var imageId = Guid.NewGuid();
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/{imageId}.png");
                using (var stream = System.IO.File.Create(uploads))
                {
                    await file.CopyToAsync(stream);
                }
                newBrand.ImageId = imageId.ToString();
            }





            await _dbContext.Brands.AddAsync(newBrand);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id, IFormFile file)
        {
            //get brand from database that id==id
            var brandDb = _dbContext.Brands.SingleOrDefault(x => x.Id == id);
            var brand = new BrandsVm()
            {
                Id = brandDb.Id,
                Name = brandDb.Name,
                Description = brandDb.Description,


            };
            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BrandsVm brand,IFormFile file)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(x => x.Id == brand.Id);
            brandDb.Name = brand.Name;
            brandDb.Description = brand.Description;
            brandDb.LastModify = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(BrandsVm brand, IFormFile file)
        {
            //get brand from Database that id==id
            var brandDb = _dbContext.Brands.SingleOrDefault(x => x.Id == brand.Id);
            brandDb.IsDelete = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }


        // this code for final delete not Activated bcs there is no final delete Interface
        public async Task<IActionResult> FinalDelete(BrandsVm brand, IFormFile file)
        {
            var brandDb = _dbContext.Brands.SingleOrDefault(x => x.Id == brand.Id);
            _dbContext.Brands.Remove(brandDb);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
