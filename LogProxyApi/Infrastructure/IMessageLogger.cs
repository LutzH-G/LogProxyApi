using LogProxyApi.Entities;

namespace LogProxyApi.Infrastructure
{
    /// <summary>
    /// Interface of message store
    /// </summary>
    public interface IMessageLogger
    {
        /// <summary>
        /// Store new message
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <returns>Stored message</returns>
        MessageDTO SendMessage(Entities.MessageDTO message);

        /// <summary>
        /// Get all stored messages
        /// </summary>
        /// <returns>Stored messages</returns>
        MessageDTO GetMessages();
    }
}
