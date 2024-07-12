using _2301B2TempEmbedding.DTOs;
using _2301B2TempEmbedding.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
                var filePath = fileName + fileExtension;
                var localPath = Path.Combine(_webRoot.WebRootPath, filePath);

                using (var stream = new FileStream(localPath,FileMode.Create))
                {
                    item.image.CopyTo(stream);
                }


                var items = new Item
                {
                    CatId = item.CatId,
                    Description = item.Description,
                    Image = localPath,
                    Name = item.Name,
                    Price = item.Price
                };

                db.Items.Add(items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var items = db.Items.Find(id);
            return View(items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Update(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Delete(int id)
        {
            var items = db.Items.Find(id);
            return View(items);
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
