using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LogProxyApi.Entities;
using LogProxyApi.Infrastructure;
using AutoMapper;

// note: Swagger uses the "required" attribute to automatically handle bad requests

namespace LogProxyApi.Controllers
{
    /// <summary>
    /// Endpoint to manage messages
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(401)]
    public class MessageController : Controller
    {
        private readonly IMessageLogger _messageLogger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor of MessageController
        /// </summary>
        /// <param name="messageLogger">Interface to store and access messages</param>
        /// <param name="mapper">DTO object mapper</param>
        public MessageController(IMessageLogger messageLogger, IMapper mapper)
        {
            _messageLogger = messageLogger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all stored messages
        /// </summary>
        /// <returns>List of stored messages</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Message> GetMessages()
        {
            var messageDto = _messageLogger.GetMessages();
            var messages = _mapper.Map<MessageDTO, IEnumerable<Message>>(messageDto);
            return messages;
        }

        /// <summary>
        /// Store new message
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <returns>Stored message</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public Message SendMessage(Message message)
        {
            var messageDto = _mapper.Map<Message, MessageDTO>(message);
            var response = _mapper.Map <MessageDTO, IEnumerable<Message>> (_messageLogger.SendMessage(messageDto));
            return response.First();
        }
    }
}
