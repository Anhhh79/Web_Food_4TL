using Microsoft.AspNetCore.Mvc;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var name = HttpContext.Session.GetString("UserName");
            if (!id.HasValue || name != "Admin")
            {
                return RedirectToAction("Index", "Error", new { area = "Customer" }); 
            }
            return View();
        }
    }
}
