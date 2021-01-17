using System;
using System.ComponentModel.DataAnnotations;

namespace LogProxyApi.Entities
{
    /// <summary>
    /// Message class
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Internal Identifier, will be set by API
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Message Title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Message Content
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Timestamp when the message was received, will be set by API
        /// </summary>
        public DateTimeOffset? ReceivedAt { get; set; }

    }
}
