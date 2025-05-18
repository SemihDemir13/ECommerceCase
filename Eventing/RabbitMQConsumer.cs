using RabbitMQ.Client; // RabbitMQ ile bağlantı kurmak için gerekli kütüphane
using RabbitMQ.Client.Events; // RabbitMQ'dan mesajları dinlemek için gerekli kütüphane
using System.Text; // Mesajı byte dizisinden string'e çevirmek için gerekli

namespace Eventing
{
    public class RabbitMQConsumer
    {
        private readonly string _hostname; // RabbitMQ sunucusunun adresi
        private readonly string _queueName; // Dinlenecek kuyruğun adı

        public RabbitMQConsumer(string hostname, string queueName)
        {
            _hostname = hostname; // Hostname'i al ve sakla
            _queueName = queueName; // Kuyruk adını al ve sakla
        }

        public void StartConsuming(Action<string> onMessageReceived)
        {
            // RabbitMQ bağlantısı için bir ConnectionFactory oluştur
            var factory = new ConnectionFactory() { HostName = _hostname };

            // Bağlantıyı oluştur (using ile otomatik dispose edilir)
            var connection = factory.CreateConnection("consumer"); // Burada bir isim veriyoruz

            // Kanalı oluştur
            var channel = connection.CreateModel();

            // Kuyruğu oluştur (eğer yoksa)
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Mesajları dinlemek için bir consumer oluştur
            var consumer = new EventingBasicConsumer(channel);

            // Mesaj geldiğinde çalışacak event handler
            consumer.Received += (model, ea) =>
            {
                // Mesajı byte dizisinden al
                var body = ea.Body.ToArray();

                // Byte dizisini string'e çevir
                var message = Encoding.UTF8.GetString(body);

                // Mesajı dışarıya ilet
                onMessageReceived(message);
            };

            // Kuyruğu dinlemeye başla
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}
