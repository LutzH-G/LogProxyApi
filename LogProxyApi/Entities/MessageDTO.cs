using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// important: The used Third Party Library requires some properties to be lower case!

namespace LogProxyApi.Entities
{
    /// <summary>
    /// Data Transfer Object of Message Class
    /// </summary>
    public class MessageDTO
    {
        /// <summary>
        /// Records
        /// </summary>
        [JsonProperty("records")]
        public IEnumerable<Record> Records { get; set; }
    }

    /// <summary>
    /// Record Class
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Fields of record
        /// </summary>
        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }

    /// <summary>
    /// Field Class
    /// </summary>
    public class Fields
    {
        /// <summary>
        /// Record Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// Record Summary
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// Record Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Timestamp when the record was received
        /// </summary>
        [JsonProperty("receivedAt")]
        public DateTimeOffset? ReceivedAt { get; set; }
    }
}
