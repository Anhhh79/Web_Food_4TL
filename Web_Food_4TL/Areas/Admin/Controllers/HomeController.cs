using Microsoft.AspNetCore.Mvc;

namespace Web_Food_4TL.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //[Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
