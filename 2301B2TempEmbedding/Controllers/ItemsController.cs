using _2301B2TempEmbedding.DTOs;
using _2301B2TempEmbedding.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.IO;

namespace _2301B2TempEmbedding.Controllers
{
    public class ItemsController : Controller
    {
        private readonly EcommerceContext db;
        private readonly IWebHostEnvironment _webRoot;

        public ItemsController(EcommerceContext _db, IWebHostEnvironment webRoot)
        {
            this.db = _db;
            _webRoot = webRoot;
        }

        public IActionResult Index()
        {
            var Itemsdata = db.Items.Include(a => a.Cat);
            return View(Itemsdata.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Category, "CatId", "CatName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemCreateDto item)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(item.image.FileName);
                var filePath = Path.Combine("uploads", fileName + fileExtension); // Ensure you have a folder named "uploads" in wwwroot
                var localPath = Path.Combine(_webRoot.WebRootPath, filePath);

                // Ensure the directory exists
                var directory = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = new FileStream(localPath, FileMode.Create))
                {
                    item.image.CopyTo(stream);
                }

                var items = new Item
                {
                    CatId = item.CatId,
                    Description = item.Description,
                    Image = "/" + filePath.Replace('\\', '/'), // Save relative path for web access
                    Name = item.Name,
                    Price = item.Price
                };

                db.Items.Add(items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(db.Category, "CatId", "CatName");
            return View(item);
        }


        public IActionResult Edit(int id)
        {
            var item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(db.Category, "CatId", "CatName", item.CatId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemCreateDto item)
        {
            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(item.image.FileName);
                var filePath = Path.Combine("uploads", fileName + fileExtension); // Ensure you have a folder named "uploads" in wwwroot
                var localPath = Path.Combine(_webRoot.WebRootPath, filePath);

                // Ensure the directory exists
                var directory = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = new FileStream(localPath, FileMode.Create))
                {
                    item.image.CopyTo(stream);
                }

                var itemInDb = db.Items.Where(x => x.Id == item.Id).FirstOrDefault();
                itemInDb.Name = item.Name;
                itemInDb.Price = item.Price;
                itemInDb.CatId = item.CatId;
                itemInDb.Description = item.Description;
                itemInDb.Image = "/" + filePath.Replace('\\', '/');

                db.Entry(itemInDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(db.Category, "CatId", "CatName");
            return View(item);
        }
    

        public IActionResult Delete(int id)
        {
            var item = db.Items.Include(x => x.Cat).Where(x => x.Id == id).FirstOrDefault();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Item item)
        {
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
