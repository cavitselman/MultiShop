using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ContactDtos;
using MS.WebUI.Services.CatalogServices.ContactServices;
using Newtonsoft.Json;
using System.Text;

namespace MS.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory3 = "İletişim";
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateContactDto createContactDto)
        {
            createContactDto.IsRead = false;
            createContactDto.SendDate = DateTime.Now;
            await _contactService.CreateContactAsync(createContactDto);
            TempData["SuccessMessage"] = "Mesajınız başarıyla gönderilmiştir!";
            return Redirect("/Contact/Index");
        }
    }
}
