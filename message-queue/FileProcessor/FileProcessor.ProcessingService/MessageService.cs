using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace FileProcessor.ProcessingService;

public class MessageService : IDisposable
{
    private const string _uri = "amqp://admin:SomeNewRabbit2020@localhost:5672";
    private IConnection _connection;
    private IModel _channel;


    public MessageService()
    {
        OpenConnection();
    }

    public void Dispose()
    {
        _connection.Close();
        _channel.Close();
    }

    private void OpenConnection()
    {
        var factory = new ConnectionFactory
        {
            Uri = new(_uri)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }


    public EventingBasicConsumer AddConsumer(string extension)
    {

        var queueName = extension;

        _channel.QueueDeclare(queueName, true, true, false);
        _channel.QueueBind(queueName, "fileProcessorEx", extension);

        Console.WriteLine($"Queue created: {extension}");

        var consumer = new EventingBasicConsumer(_channel);
        
        _channel.BasicConsume(queueName, true, consumer);

        return consumer;
    }


}

