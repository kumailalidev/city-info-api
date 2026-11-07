namespace CityInfo.API.Services;

/// <summary>
/// Sends email messages through the cloud-based mail service.
/// </summary>
public class CloudMailService : IMailService
{
    private readonly string _mailTo = "admin@mycompany.com";
    private readonly string _mailFrom = "noreply@company.com";

    /// <summary>
    /// Sends an email message to the configured recipient.
    /// </summary>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The body of the email.</param>
    public void Send(string subject, string message)
    {
        // send mail - output to console window
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(CloudMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}