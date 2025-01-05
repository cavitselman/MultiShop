using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult _UILayout()
        {
            return View();
        }
    }
}
