using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult _UILayout()
        {
            return View();
        }
        public PartialViewResult NavbarPartial()
        {
            return PartialView();
        }

        public PartialViewResult TopbarPartial()
        {
            return PartialView();
        }
    }
}
