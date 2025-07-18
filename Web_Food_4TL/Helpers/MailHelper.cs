using System.Net.Mail;
using System.Net;

public class MailHelper
{
    private readonly IConfiguration _configuration;

    public MailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendOrderConfirmation(string toEmail, string subject, string body)
    {
        var fromEmail = _configuration["SmtpSettings:From"];
        var smtpServer = _configuration["SmtpSettings:Host"];
        var smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        var smtpUser = _configuration["SmtpSettings:Username"];
        var smtpPass = _configuration["SmtpSettings:Password"];


        var message = new MailMessage(fromEmail, toEmail, subject, body);
        message.IsBodyHtml = true;

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;
            client.Send(message);
        }
    }
}
