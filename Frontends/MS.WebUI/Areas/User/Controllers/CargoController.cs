﻿using Microsoft.AspNetCore.Mvc;

namespace MS.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
