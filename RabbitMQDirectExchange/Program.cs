using RabbitMQ.Client;
using System.Text;

//6 DERS

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection = factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();

// 1 Parametre exchange adı ikinci parametre exchange tipi, tipini Direct Exchange olarak ayarladik
// Burada exchange oluşturuyoruz

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    //Basic Publish mesajı yayınlıyan metod
    //1 Parametre de hangi exchange kullanacağını bildiriyoruz
    //2 Parametre de Mesajların bir exchange üzerinden doğru kuyruğa yönlendirilmesini sağlayan bir anahtardır.
    //3 Parametre gönderilecek mesaj
    channel.BasicPublish(exchange: "direct-exchange-example", routingKey: "direct-exchange-example", body: byteMessage);
}