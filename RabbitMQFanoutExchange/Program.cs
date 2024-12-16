using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection = factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();

//Exchange olusturuyoruz

//1 Parametre exchange adi
//2 Parametre exchange tipi
channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");

    //Burada routingKey null verme sebebimiz FanoutExchange zaten kendine baglanmis olan yani bind olmus olan tum kuyruklara
    // Isim gozetmeksizin deger gonderecek oldugu icin routingKey adi bos bırakilir.
    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        routingKey: string.Empty,
        body: message);
}

Console.Read();