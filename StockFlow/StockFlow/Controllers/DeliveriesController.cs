using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.Data;
using StockFlow.Models;
using StockFlow.Models.ViewModels;


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
            var deliveries = await _context.Deliveries.ToListAsync();
            return View(deliveries);
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

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDeliveryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var delivery = new Delivery
            {
                DeliveryDate = viewModel.DeliveryDate,
                SupplierName = viewModel.SupplierName,
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
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}