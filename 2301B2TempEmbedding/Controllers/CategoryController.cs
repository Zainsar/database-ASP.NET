using _2301B2TempEmbedding.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2301B2TempEmbedding.Controllers
{
    public class CategoryController : Controller
    {
        private readonly EcommerceContext db;
        public CategoryController(EcommerceContext _db)
        {

            db = _db;

        }
        public IActionResult Index()
        {
            var cat = db.Category.ToList();
            return View(cat);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var cat = db.Category.Find(id);
            return View(cat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Category.Update(cat);
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
            var cat = db.Category.Find(id);
            return View(cat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category cat)
        {
            db.Category.Remove(cat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
