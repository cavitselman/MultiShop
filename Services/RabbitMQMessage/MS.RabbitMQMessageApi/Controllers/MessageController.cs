using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MS.RabbitMQMessageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateMessage()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            var connection = await connectionFactory.CreateConnectionAsync();

            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync("Kuyruk1", false, false, false, arguments: null);
            var messageContent = "Merhaba Bu Bir RabbitMQ Kuyruk Mesajıdır";
            var byteMessageContent = Encoding.UTF8.GetBytes(messageContent);
            await channel.BasicPublishAsync(exchange: "", routingKey: "Kuyruk1", body: byteMessageContent);
            return Ok("Mesajınız Kuyruğa Alınmıştır");
        }

        private static string message;

        [HttpGet]
        public async Task<IActionResult> ReadMessage()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, x) =>
            {
                var byteMessage = x.Body.ToArray();
                message = Encoding.UTF8.GetString(byteMessage);
            };

            await channel.BasicConsumeAsync(queue: "Kuyruk1", autoAck: false, consumer: consumer);

            if (string.IsNullOrEmpty(message))
            {
                return NoContent();
            }
            else
            {
                return Ok(message);
            }
        }
    }
}
