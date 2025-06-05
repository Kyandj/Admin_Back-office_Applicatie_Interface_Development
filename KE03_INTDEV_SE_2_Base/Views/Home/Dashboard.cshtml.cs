using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Models;


public class DashboardModel : PageModel
{
    private readonly MatrixIncDbContext _context;

    public DashboardModel(MatrixIncDbContext context)
    {
        _context = context;
    }

    public List<DashboardItem> DashboardItems { get; set; }

    public async Task OnGetAsync()
    {
        DashboardItems = await _context.OrderItem
            .Include(oi => oi.Order)
                .ThenInclude(o => o.Customer)
            .Include(oi => oi.Product)
            .Select(oi => new DashboardItem
            {
                CustomerName = oi.Order.Customer.Name,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                StockLeft = oi.Product.Stock
            })
            .ToListAsync();
    }
}

// Zet deze klasse buiten DashboardModel!
public class DashboardItem
{
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public int StockLeft { get; set; }
}

