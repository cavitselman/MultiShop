using Microsoft.AspNetCore.Mvc;
using MS.DtoL.MessageDtos;
using MS.WebUI.Areas.Admin.Models;
using MS.WebUI.Services.MessageServices;
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

        public async Task<IActionResult> MessageDetail(int messageId)
        {
            string currentUserId = "6f856e19-216a-428d-998d-01408f46b18d";

            var inbox = await _messageService.GetInboxMessageAsync(currentUserId);
            var sendbox = await _messageService.GetSendboxMessageAsync(currentUserId);

            // Inbox'tan mesajı bul
            var inboxMessage = inbox.FirstOrDefault(m => m.UserMessageId == messageId);

            ResultInboxMessageDto message;

            if (inboxMessage != null)
            {
                message = inboxMessage;
            }
            else
            {
                // Sendbox'tan mesajı bul
                var sendboxMessage = sendbox.FirstOrDefault(m => m.UserMessageId == messageId);
                if (sendboxMessage == null)
                    return NotFound();

                // Sendbox mesajını inbox DTO'ya dönüştür
                message = new ResultInboxMessageDto
                {
                    UserMessageId = sendboxMessage.UserMessageId,
                    SenderId = sendboxMessage.SenderId,
                    ReceiverId = sendboxMessage.ReceiverId,
                    Subject = sendboxMessage.Subject,
                    MessageDetail = sendboxMessage.MessageDetail,
                    IsRead = sendboxMessage.IsRead,
                    MessageDate = sendboxMessage.MessageDate
                };
            }

            // View'a tek mesaj olarak liste halinde gönder (varsa)
            return View(new List<ResultInboxMessageDto> { message });
        }

        //[HttpPost]
        //public async Task<IActionResult> SendMessage(string ReceiverId, string MessageDetail)
        //{
        //    var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var message = new CreateMessageDto
        //    {
        //        SenderId = senderId,
        //        ReceiverId = ReceiverId,
        //        Subject = "-", // veya konu alanı eklersin
        //        MessageDetail = MessageDetail,
        //        MessageDate = DateTime.Now,
        //        IsRead = false
        //    };

        //    await _messageService.CreateMessageAsync(message);
        //    return RedirectToAction("Chat", new { userId = ReceiverId });
        //}
    }
}
