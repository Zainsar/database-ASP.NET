using _2301B2TempEmbedding.Models;

using Microsoft.AspNetCore.Mvc;

namespace _2301B2TempEmbedding.Controllers
{
    public class productController : Controller
    {
        private readonly EcommerceContext db;
        public productController(EcommerceContext _db)
        {

            db = _db;

        }
        public IActionResult Index()
        {
            var tables = db.Tables.ToList();
            return View(tables);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Table tab)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Add(tab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var table = db.Tables.Find(id);
            return View(table);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Table tab)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Update(tab);
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
            var table = db.Tables.Find(id);
            return View(table);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Table tab)
        {
            
                db.Tables.Remove(tab);
                db.SaveChanges();
                return RedirectToAction("Index");
            

        }
    }
}
