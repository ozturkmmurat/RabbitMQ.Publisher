using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection = factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Mesajın gönderileceği Topic formatını belirtiniz : ");
    string topic = Console.ReadLine();
    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message);
}

Console.Read();