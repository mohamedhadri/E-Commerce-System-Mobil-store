using FinalProject.Areas.Identity.Data;
using FinalProject.Vm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Models;
using FinalProject.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace FinalProject.Controllers
{
    public class UserController : Controller
    {

        private readonly FinalProjectContext _context;

        public UserController(FinalProjectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var listcollection = _context.Items.Where(x => x.CategoryId==7 && x.IsDelete == false).Select(x => new ItemsVm()
            {
                Id= x.Id,
                Name= x.Name,
                Price=x.Price,
                ImageId= _context.Attachments.FirstOrDefault(r=> r.RecordId==x.Id.ToString()&& r.RecordType== RecordType.Items).FileName
            }).ToList();


            var listItem = _context.Items.Where(x =>  x.IsDelete == false && x.CategoryId == 7).Take(6).Select(x => new ItemsVm()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                ImageId = _context.Attachments.FirstOrDefault(r => r.RecordId == x.Id.ToString() && r.RecordType == RecordType.Items).FileName
            }).ToList();

            return View(new HomeVm() { ListCollections=listcollection,ListItems=listItem});
        }
   

          public async Task<IActionResult> Detail(int id)
           {
        var itemDb = await _context.Items.Include(x => x.Brands).Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        var itemVm = new ItemsVm()
        {
           Id = itemDb.Id,
            Name = itemDb.Name,
            Description = itemDb.Description,
            ShortDescription = itemDb.ShortDescription,
           Price = itemDb.Price,
            BrandName= itemDb.Brands.Name,
            CategoryName= itemDb.Categories.Name,
            Images= _context.Attachments.Where(a=> a.RecordId==itemDb.Id.ToString() && a.RecordType==RecordType.Items).Select(x=> x.FileName).ToList()
        };

        return View(itemVm);
        }




        public async Task<IActionResult> CheckOut(int id)
        {
          

            var checkOutVm = new CheckOutVm
            {

                ItemId = id
            };

            return View(checkOutVm);

        }


        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutVm checkOut)
        {

            await _context.Orders.AddAsync(new Orders()
            { FullName = checkOut.FullName, Phone = checkOut.Phone, Address = checkOut.Address, ItemId = checkOut.ItemId, Email= checkOut.Email });

            await _context.SaveChangesAsync();
                return RedirectToAction("Index");
        }









    }


}
