using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.ContactDtos;
using MS.WebUI.Services.CatalogServices.ContactServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _contactService.GetAllContactAsync();
            return View(messages);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var message = await _contactService.GetByIdContactAsync(id);
            if (message == null)
                return NotFound();

            if (!message.IsRead)
            {
                var updateDto = new UpdateContactDto
                {
                    ContactId = message.ContactId,
                    NameSurname = message.NameSurname,
                    Email = message.Email,
                    Subject = message.Subject,
                    Message = message.Message,
                    IsRead = true,
                    SendDate = message.SendDate
                };

                await _contactService.UpdateContactAsync(updateDto);
            }

            return View(message);
        }
    }
}
