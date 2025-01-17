
using EmailService;

public class MessageCheckService : BackgroundService
{
    private readonly IEmailSender _emailSender;
    private readonly List<Message> _messages; // Хранилище сообщений

    public MessageCheckService(ILogger<MessageCheckService> logger, IEmailSender emailSender)
    {
        _emailSender = emailSender;
        _messages = new List<Message>();
    }

    public void AddMessage(Message message)
    {
        _messages.Add(message);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            CheckMessages();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private void CheckMessages()
    {
        var now = DateTime.Now;
        
        var readyMessages = _messages.Where(m => m.SendDate <= now).ToList();

        foreach (var message in readyMessages)
        {
            try
            {
                _emailSender.SendEmail(message);
                
                _messages.Remove(message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
