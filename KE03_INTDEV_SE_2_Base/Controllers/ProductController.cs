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
        public async Task<IActionResult> AlleProducten()
        {
            var producten = await _context.Products.ToListAsync();
            return View(producten);
        }
    }
}

