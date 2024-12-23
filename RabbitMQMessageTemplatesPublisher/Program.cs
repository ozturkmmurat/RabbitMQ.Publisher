using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection = factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();

#region P2P (Point-to-Point) Tasarımı

////Kuyruk adını verdik
//string queueeName = "example-p2p-queue";

////Kuyruğu oluşturduk
//channel.QueueDeclare(
//    queue: queueeName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

////Mesajı oluşturduk
//byte[] message = Encoding.UTF8.GetBytes("Merhaba");

////Mesajı yayınladık
//channel.BasicPublish(
//    exchange:string.Empty,
//    routingKey: queueeName,
//    body:message
//    );
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı

//Exchange adını verdik
//string exchangeName = "example-pub-sub-exchange";

////Exchange oluşturduk
//channel.ExchangeDeclare(
//    exchange:exchangeName,
//    type:ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);  
//    //Mesaj oluşturuyoruz
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);

//    //Mesajı yayınladık
//    //Kuyruk adı gözetkmesizin fanout kullandığımız için kuyruk adı girmiyoruz. Çünkü exchange bind olmuş tüm kuyruklara gidecek
//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: message
//        );
//}
#endregion


#region Work Queue(İş Kuyruğu Tasarımı)

//// Kuyruk adı oluşturuldu
//string queueName = "example-work-queue";

////Kuyruk oluşturuyoruz
//channel.QueueDeclare(
//    queue:queueName,
//    durable:false,
//    exclusive:false,
//    autoDelete:false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    //Mesaj oluşturuyoruz
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);

//    //Mesajı yayınladık
//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message
//        );

#endregion

#region Request/Response Tasarımı

//Kuyruk adını oluşturuyoruz
string requestQueueName = "example-request-response-queue";

//Kuyruğu oluşturuyoruz
channel.QueueDeclare(
    queue:requestQueueName,
    durable:false,
    exclusive:false,
    autoDelete:false);

//Response kuyruğu oluşturuyoruz
string replyQueueName = channel.QueueDeclare().QueueName;

//CorelllationId oluşturuyoruz publisher için
string correlationId = Guid.NewGuid().ToString();

#region Request Mesajını Oluşturma ve Göndrme

IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion

#region Response Kuyruğu Dinleme
EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue:replyQueueName,
    autoAck:true,
    consumer:consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};
#endregion

#endregion

Console.Read();
