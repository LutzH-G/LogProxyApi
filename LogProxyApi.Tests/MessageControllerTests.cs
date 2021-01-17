using AutoMapper;
using LogProxyApi.Controllers;
using LogProxyApi.Infrastructure;
using NUnit.Framework;
using Moq;
using LogProxyApi.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;

// note: BlackBox tests 
// integration tests should be implemented - it was left out to avoid spaming the database

namespace LogProxyApi.Tests
{
    public class MessageControllerTests
    {
        private MessageController _controller;
        private IMapper _mapper;
        private const int STORED_RECORDS = 5;

        #region Setup

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new LogMapperProfile()));
            _mapper = mapperConfig.CreateMapper();

            var mockMessageLogger = new Mock<IMessageLogger>();
            mockMessageLogger.Setup(logger => logger.GetMessages()).Returns(GetTestMessageDTO());
            mockMessageLogger.Setup(logger => logger.SendMessage(It.IsAny<MessageDTO>())).Returns<MessageDTO>(input => input);

            _controller = new MessageController(mockMessageLogger.Object, _mapper);
        }

        #endregion

        #region Test Cases

        [Test]
        public void TestGetMessages()
        {
            var response =_controller.GetMessages();
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() == STORED_RECORDS);

            // compare expected response with actual response, but ignore Timestamps
            var responseJson = JsonConvert.SerializeObject(response.Select(UnsetTimestamp));
            var expectedResponse = _mapper.Map<MessageDTO, IEnumerable<Message>>(GetTestMessageDTO());
            var expectedJson = JsonConvert.SerializeObject(expectedResponse.Select(UnsetTimestamp));
            Assert.AreEqual(expectedJson, responseJson);
        }

        [Test]
        public void TestSendMessage()
        {
            var testMessage = new Message
            {
                Text = "Test Message",
                Title = "Test Title",
            };
            var response = _controller.SendMessage(testMessage);
            Assert.IsNotNull(response);
            Assert.IsTrue(!string.IsNullOrEmpty(response.Id));
            Assert.IsTrue(response.ReceivedAt < DateTimeOffset.Now.AddSeconds(1) && response.ReceivedAt > DateTimeOffset.Now.AddSeconds(-1));

            Assert.AreEqual(testMessage.Text, response.Text);
            Assert.AreEqual(testMessage.Title, response.Title);
        }

        [Test]
        public void TestSendMessageWithTimestampAndId()
        {
            var testMessage = new Message
            {
                Id = "123",
                Text = "Test Message",
                Title = "Test Title",
                ReceivedAt = DateTimeOffset.Now.AddDays(-1)
            };
            var response = _controller.SendMessage(testMessage);
            Assert.IsNotNull(response);
            Assert.IsTrue(!string.IsNullOrEmpty(response.Id));
            Assert.IsTrue(response.ReceivedAt < DateTimeOffset.Now.AddSeconds(1) && response.ReceivedAt > DateTimeOffset.Now.AddSeconds(-1));

            Assert.AreEqual(testMessage.Text, response.Text);
            Assert.AreEqual(testMessage.Title, response.Title);
            Assert.AreNotEqual(testMessage.Id, response.Id);
            Assert.AreNotEqual(testMessage.ReceivedAt, response.ReceivedAt);
        }

        [Test]
        public void CheckGeneratedId()
        {
            // represents the expected behaviour of generated ids
            for (var i = 0; i < 3; i++)
                SendAndCheckMessageForId(i.ToString());
        }

        #endregion

        #region Private Utility Methods

        private void SendAndCheckMessageForId(string id)
        {
            var testMessage = new Message
            {
                Text = "Test Message",
                Title = "Test Title",
            };
            var response = _controller.SendMessage(testMessage);
            Assert.IsNotNull(response);
            Assert.AreEqual(id, response.Id);
        }

        private MessageDTO GetTestMessageDTO()
        {
            var testMessageDTO = new MessageDTO
            {
                Records = new List<Record>()
            };

            for(var i = 0; i < STORED_RECORDS; i++)
            {
                ((List<Record>)testMessageDTO.Records).Add(new Record
                {
                    Fields = new Fields
                    {
                        Id = i.ToString(),
                        Message = "TestMessage",
                        ReceivedAt = DateTimeOffset.Now,
                        Summary = "Title",
                    }
                });
            }
            return testMessageDTO;
        }

        private Message UnsetTimestamp(Message message)
        {
            return new Message
            {
                Id = message.Id,
                ReceivedAt = null,
                Text = message.Text,
                Title = message.Title,
            };
        }
        #endregion
    }
}