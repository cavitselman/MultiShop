using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
