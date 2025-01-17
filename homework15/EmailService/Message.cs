using MimeKit;

namespace EmailService;

public class Message
{
    public string Subject { get; set; } = "FutureMe";
    public string Content { get; set; }
    public DateTime SendDate { get; set; }
    public string Email { get; set; }
}


