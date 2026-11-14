using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockFlow.Data;
namespace StockFlow.Controllers
{
    public class StockTransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public StockTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, string transactionType, int page = 1)
        {
            var transactions = _context.StockTransactions
                .Include(t => t.Product)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                transactions = transactions.Where(t =>
                    t.Product.Name.Contains(searchString));
            }
            if (!string.IsNullOrWhiteSpace(transactionType))
            {
                transactions = transactions.Where(t =>
                    t.TransactionType == transactionType);
            }
            var totalRecords = await transactions.CountAsync();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
            if (totalPages > 0)
            {
                page = Math.Clamp(page, 1, totalPages);
            }
            else
            {
                page = 1;
            }
            var transactionList = await transactions
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.TransactionType = transactionType;

            return View(transactionList);
        }
    }
}
