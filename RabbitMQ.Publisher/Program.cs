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
channel.QueueDeclare(queue:"example-queue", exclusive:false, durable:true);

//Queue'ya Mesaj Gonderme

//RabbitMQ kuyruğa gönderilecek mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte dönüştürmemiz gerekmektedir.

//Kuyruktaki mesajı kalıcı hale getiriyoruz.
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

//Herhangi bir exchange belirtmeidgimiz icin rabbitmq direcktExchange kullanacaktir.
for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba RabbitMQ" + i);
    //BasicProperties ile kuyruktaki mesajı kalıcı hale getirdik.
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties:properties);
}

Console.Read();


