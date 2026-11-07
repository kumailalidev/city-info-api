namespace CityInfo.API.Services;

/// <summary>
/// Provides a local mail service implementation that writes messages to the console.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LocalMailService"/> class.
/// </remarks>
/// <param name="configuration">The application configuration containing the mail settings.</param>
public class LocalMailService(IConfiguration configuration) : IMailService
{
    private readonly string _mailTo = configuration["mailSettings:mailToAddress"]
            ?? throw new ArgumentNullException("mailSettings:mailToAddress");
    private readonly string _mailFrom = configuration["mailSettings:mailFromAddress"]
            ?? throw new ArgumentNullException("mailSettings:mailFromAddress");

    /// <summary>
    /// Sends a mail message.
    /// </summary>
    /// <param name="subject">The subject of the message.</param>
    /// <param name="message">The body of the message.</param>
    public void Send(string subject, string message)
    {
        // send mail - output to console window
        Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
    }
}