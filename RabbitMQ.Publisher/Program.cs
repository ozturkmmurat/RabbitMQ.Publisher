using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
//Baglanti Olusturma
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktiflestirme ve Kanal Acma
using IConnection connection =  factory.CreateConnection();
//Kanal olusturma
using IModel channel = connection.CreateModel();


//Queue Olusturma
//Durable true vererek kuyruğu kalıcı olarak işaretledik
//channel.QueueDeclare(queue:"example-queue", exclusive:false, durable:true);

//Queue'ya Mesaj Gonderme

//RabbitMQ kuyruğa gönderilecek mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte dönüştürmemiz gerekmektedir.

//Kuyruktaki mesajı kalıcı hale getiriyoruz.
//IBasicProperties properties = channel.CreateBasicProperties();
//properties.Persistent = true;

//Herhangi bir exchange belirtmeidgimiz icin rabbitmq direcktExchange kullanacaktir.
//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba RabbitMQ" + i);
//    //BasicProperties ile kuyruktaki mesajı kalıcı hale getirdik.
//    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties:properties);
//}

//6 DERS

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

Console.Read();


