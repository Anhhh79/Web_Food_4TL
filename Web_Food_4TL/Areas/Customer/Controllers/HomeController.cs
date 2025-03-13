using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_Food_4TL.Data;
using Web_Food_4TL.Models;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var monAnList = _context.MonAns.ToList();
            return View(monAnList);
        }

        public IActionResult GetMonAnDetail(int id)
        {
            var monAn = _context.MonAns
                .Include(m => m.DanhMuc)
                .Include(m => m.AnhMonAnh)
                .FirstOrDefault(m => m.Id == id);

            if (monAn == null)
            {
                return NotFound();
            }

            return PartialView("_MonAnDetailPartial", monAn);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
