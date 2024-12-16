using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection = factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange-example",
    type: ExchangeType.Headers);


for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Lütfen header value'sunu giriniz : ");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>() {
        ["no"] = value
    };

    channel.BasicPublish(
        exchange:"header-exchange-example",
        routingKey: string.Empty,
        body : message,
        basicProperties : basicProperties
        );
}

Console.Read();
