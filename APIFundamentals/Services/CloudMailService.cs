namespace APIFundamentals.Services;

public class CloudMailService: IMailService
{
    private readonly string _mailTo = string.Empty;
    private readonly string _mailFrom = string.Empty;
    
    public CloudMailService(IConfiguration configuration)
    {
        _mailTo = configuration["MailSettings:mailTo"];
        _mailFrom = configuration["MailSettings:mailFrom"];
    }

    public void SendMail(string subject, string message)
    {
        //sending mail- output to Console window 
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $"with {nameof(CloudMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}