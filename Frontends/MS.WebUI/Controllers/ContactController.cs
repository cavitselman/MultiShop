using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
