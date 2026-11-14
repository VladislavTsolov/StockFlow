using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.Data;
using StockFlow.Models;
using StockFlow.Models.ViewModels;
using System.Linq;

namespace StockFlow.Controllers
{
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var deliveries = await _context.Deliveries.Include(d => d.Supplier).ToListAsync();
            return View(deliveries);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.Include(d => d.Supplier).FirstOrDefaultAsync(d => d.Id == id);

            if (delivery == null)
            {
                return NotFound();
            }

            var items = await _context.DeliveryItems
                .Include(di => di.Product)
                .Where(di => di.DeliveryId == id)
                .ToListAsync();

            ViewBag.Items = items;

            return View(delivery);
        }
        public async Task<IActionResult> Create()
        {
            var products = await _context.Products.ToListAsync();

            var viewModel = new CreateDeliveryViewModel
            {
                DeliveryDate = DateTime.Today,
                Products = products.Select(p => new DeliveryProductViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Quantity = 0
                }).ToList()
            };

            return View("Create", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDeliveryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var products = await _context.Products.ToListAsync();

                viewModel.Products = products.Select(p => new DeliveryProductViewModel
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Quantity = viewModel.Products?.FirstOrDefault(item => item.ProductId == p.Id)?.Quantity ?? 0
                }).ToList();

                return View(viewModel);
            }

            var supplierName = viewModel.SupplierName.Trim();

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.CompanyName == supplierName);

            if (supplier == null)
            {
                supplier = new Supplier
                {
                    CompanyName = supplierName
                };

                _context.Suppliers.Add(supplier);
            }

            var delivery = new Delivery
            {
                DeliveryDate = viewModel.DeliveryDate,
                Supplier = supplier,
                Notes = viewModel.Notes
            };

            _context.Deliveries.Add(delivery);

            foreach (var item in viewModel.Products)
            {
                if (item.Quantity > 0)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                    {
                        continue;
                    }

                    var deliveryItem = new DeliveryItem
                    {
                        Delivery = delivery,
                        ProductId = item.ProductId,
                        QuantityDelivered = item.Quantity
                    };

                    _context.DeliveryItems.Add(deliveryItem);

                    product.QuantityInStock += item.Quantity;
                    var stockTransaction = new StockTransaction
                    {
                        ProductId = product.Id,
                        TransactionType = "Incoming",
                        Quantity = item.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        Description = "Stock received through delivery"
                    };

                    _context.StockTransactions.Add(stockTransaction);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}