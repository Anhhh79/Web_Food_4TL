using Microsoft.AspNetCore.Mvc;

namespace Web_Food_4TL.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCart : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
