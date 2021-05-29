using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Data;
using FinalProject.Vm;
using FinalProject.Enum;
using Microsoft.AspNetCore.Http;
using FinalProject.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;




namespace FinalProject.Controllers
{
    public class CategoriesController : Controller
    {

        private FinalProjectContext _dbContext;
        private readonly IHostEnvironment _hostEnvironment;

        public CategoriesController(FinalProjectContext dbContext, IHostEnvironment hostEnviornment)
        {
            _dbContext = dbContext;
            _hostEnvironment = hostEnviornment;
        }


        public IActionResult Index()
        {
            //get the category that not deleted
            var categories = _dbContext.Categories.Where(x => x.IsDelete == false);
            //mapping to CategoryVm
            var list = _dbContext.Categories.Where(x => x.IsDelete == false).Select(x => new CategoryVm
            {
                Id = x.Id,
                Name= x.Name,
                CreationDate=x.CreateDate,
                ImageId = _dbContext.Attachments.FirstOrDefault(r => r.RecordId == x.Id.ToString() && r.RecordType == RecordType.Categories)
                .FileName
            }) ;
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryVm category, List<IFormFile>files)
        {
         var entity=   await _dbContext.Categories.AddAsync(category.ToEntity());
            await _dbContext.SaveChangesAsync();

            var CategoryDb = category.ToEntity();

            if (files != null)
                if (files.Count > 0)
                {
                    foreach(var file in files)
                    {
                        //generate random string for image id
                        var imageId = Guid.NewGuid();
                          var uploads = Path.Combine( _hostingEnvironment.WebRootPath, $"uploads/Categories/{imageId}.png");
                          using (var stream = System.IO.File.Create(uploads))
                          {
                           await file.CopyToAsync(stream);
                          }
                        var image = new Attachments()
                        {
                            RecordId=entity.Entity.Id.ToString(),
                            FileName=imageId.ToString(),
                            RecordType=RecordType.Categories
                        };
                    }
                }


                    return RedirectToAction("Index");


           
                    //  {
                    
                    // 
                    //  category.ImageId = imageId.ToString();
                    // }
                   // var newBrand = new Categories() { Name = category.Name, Description = category.Description };
        }

    }
}
