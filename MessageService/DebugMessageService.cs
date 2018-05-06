using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EnarcLabs.Technonomicon.Daemon.MessageService
{
    /// <summary>
    /// Provides IMessageService by writing message attempts to the console.
    /// </summary>
    /// <inheritdoc cref="IMessageService"/>
    public class DebugMessageService : IMessageService
    {
        public async Task<SendResults> Send(MailMessage mail)
        {
            Debug.WriteLine($"Sending a message to ({string.Join("; ", mail.To.Select(x => x.Address))}): \"{mail.Body}\"");

            return SendResults.Success;
        }
    }
}
