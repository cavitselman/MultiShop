using Microsoft.AspNetCore.Mvc;
using MS.DtoL.MessageDtos;
using MS.WebUI.Areas.Admin.Models;
using MS.WebUI.Services.MessageServices;
using System.Linq;
using System.Security.Claims;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            string adminId = "6f856e19-216a-428d-998d-01408f46b18d";
            var inboxMessages = await _messageService.GetInboxMessageAsync(adminId);
            return View(inboxMessages);
        }

        [HttpGet("/Admin/Message/MessageDetail/{messageId}")]
        public async Task<IActionResult> MessageDetail(int messageId)
        {
            string currentUserId = "6f856e19-216a-428d-998d-01408f46b18d"; // Admin veya giriş yapan kullanıcı

            var inbox = await _messageService.GetInboxMessageAsync(currentUserId);
            var sendbox = await _messageService.GetSendboxMessageAsync(currentUserId);

            // Mesajı inbox'tan al
            var message = inbox.FirstOrDefault(m => m.UserMessageId == messageId);

            if (message == null)
                return NotFound();

            var replies = sendbox.Where(s => s.ReceiverId == message.SenderId && s.SenderId == currentUserId).OrderBy(s => s.MessageDate).ToList();

            ViewBag.Replies = replies;

            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAjax(string ReceiverId, string MessageDetail, string Subject, int ParentMessageId)
        {
            var senderId = "6f856e19-216a-428d-998d-01408f46b18d";
            var senderUserName = User.Identity?.Name ?? "Admin";

            var message = new CreateMessageDto
            {
                SenderId = senderId,
                ReceiverId = ReceiverId,
                Username = senderUserName,
                Subject = Subject,
                MessageDetail = MessageDetail,
                MessageDate = DateTime.UtcNow,
                IsRead = false
            };

            await _messageService.CreateMessageAsync(message);

            // Ajax’a JSON olarak gönder
            return Json(new
            {
                success = true,
                username = senderUserName,
                messageDetail = MessageDetail,
                messageDate = DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm")
            });
        }

    }
}
