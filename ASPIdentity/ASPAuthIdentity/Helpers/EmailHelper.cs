using MailKit.Net.Smtp;
using MimeKit;

namespace ASPAuthIdentity.Helpers;

public class EmailHelper
{
    public async Task<bool> SendEmail(string email, string link, string subject)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ASP.NET Identity", "pantera197618@gmail.com"));
        message.To.Add(new MailboxAddress(name: email, address: email));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = link,
        };
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("pantera197618@gmail.com", "cowi taof vmlr txkd");

            try
            {
                await client.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        return false;
    }
}