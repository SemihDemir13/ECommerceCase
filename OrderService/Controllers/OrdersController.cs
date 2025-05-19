using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eventing;
using Libraries;
using Libraries.OrderServices.Entities;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            // Sipariş oluşturma işlemleri burada yapılır (şimdilik dummy)
            // RabbitMQ'ya mesaj gönder
            var publisher = new RabbitMQPublisher("localhost", "orderqueue");
            publisher.Publish($"Yeni sipariş oluşturuldu: {order.OrderId}");

            return Ok("Sipariş oluşturuldu ve mesaj kuyruğa gönderildi.");
        }
    }
}


