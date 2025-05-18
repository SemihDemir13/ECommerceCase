using RabbitMQ.Client; // RabbitMQ ile bağlantı kurmak için gerekli kütüphane
using System.Text; // Mesajı byte dizisine çevirmek için gerekli

namespace Eventing
{
    public class RabbitMQPublisher
    {
        private readonly string _hostname; // RabbitMQ sunucusunun adresi
        private readonly string _queueName; // Mesaj gönderilecek kuyruğun adı

        public RabbitMQPublisher(string hostname, string queueName)
        {
            _hostname = hostname; // Hostname'i al ve sakla
            _queueName = queueName; // Kuyruk adını al ve sakla
        }

        public void Publish(string message)
        {
            // RabbitMQ bağlantısı için bir ConnectionFactory oluştur
            var factory = new ConnectionFactory() { HostName = _hostname };

            // Bağlantıyı oluştur (using ile otomatik dispose edilir)
            using var connection = factory.CreateConnection();

            // Kanalı oluştur (using ile otomatik dispose edilir)
            using var channel = connection.CreateModel();

            // Kuyruğu oluştur (eğer yoksa)
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Mesajı byte dizisine çevir
            var body = Encoding.UTF8.GetBytes(message);

            // Mesajı kuyruğa gönder
            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
        }
    }
}
