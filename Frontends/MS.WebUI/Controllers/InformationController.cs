using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Controllers
{
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
