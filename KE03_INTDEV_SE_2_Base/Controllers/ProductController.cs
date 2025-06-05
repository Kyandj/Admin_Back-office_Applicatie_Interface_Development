using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KE03_INTDEV_SE_2_Base.Controllers
{
    public class ProductController : Controller
    {
        private readonly MatrixIncDbContext _context;

        public ProductController(MatrixIncDbContext context)
        {
            _context = context;
        }

        // GET: Producten/ProductBewerken/5
        public async Task<IActionResult> ProductBewerken(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Producten/ProductBewerken/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductBewerken(int id, [Bind("Id,Name,Category,Price,ArticleNumber,Stock,Description,IsVisible,ImageUrl")] Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AlleProducten", "Product");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(product);
        }

        // AlleProducten pagina
        public async Task<IActionResult> AlleProducten(string categorie)
        {
            // Haal alle unieke categorieën op voor de view
            var categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            // Filter producten op categorie als die is opgegeven
            var producten = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(categorie))
            {
                producten = producten.Where(p => p.Category == categorie);
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = categorie;

            return View(await producten.ToListAsync());
        }



        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Voeg product toe aan database
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(AlleProducten));
            }
            return View(product);
        }

        // Product details pagina
        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AlleProducten));
        }

    }
}

