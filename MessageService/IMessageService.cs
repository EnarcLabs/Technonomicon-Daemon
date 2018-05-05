using System.Net.Mail;
using System.Threading.Tasks;

namespace EnarcLabs.Technonomicon.Daemon.MessageService
{
    public interface IMessageService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="mail">The message to send.</param>
        /// <returns>A task for the send operation.</returns>
        Task<SendResults> Send(MailMessage mail);
    }

    
    public enum SendResults
    {
        Success = 0,
        Error = 1,
    }
}
