namespace CityInfo.API.Services;

/// <summary>
/// Provides functionality to send email messages.
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Sends an email message with the specified subject and content.
    /// </summary>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The body of the email.</param>
    void Send(string subject, string message);
}