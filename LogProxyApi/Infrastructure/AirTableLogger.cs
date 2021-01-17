using LogProxyApi.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace LogProxyApi.Infrastructure
{
    /// <summary>
    /// Concrete implementation of AirTable
    /// </summary>
    public class AirTableLogger : IMessageLogger
    {
        private readonly string _getMessageUrl;
        private readonly string _postMessageUrl;
        private readonly string _authKey;

        /// <summary>
        /// Constructor of AirTableLogger
        /// </summary>
        /// <param name="configuration">App Configuration</param>
        public AirTableLogger(IConfiguration configuration)
        {
            _getMessageUrl = configuration.GetConnectionString("AirTableGetMessageUrl");
            _postMessageUrl = configuration.GetConnectionString("AirTableSendMessageUrl");
            _authKey = configuration["AirTableAuthenticationKey"];
        }

        /// <summary>
        /// Get all stored messages
        /// </summary>
        /// <returns>Stored messages</returns>
        public MessageDTO GetMessages()
        {
            var restClient = new RestClient(_getMessageUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", $"Bearer {_authKey}");

            var response = restClient.Execute<MessageDTO>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;

            throw GenerateWrongResponseException(response);
        }

        /// <summary>
        /// Store new message
        /// </summary>
        /// <param name="message">Message to be stored</param>
        /// <returns>Stored message</returns>
        public MessageDTO SendMessage(MessageDTO message)
        {
            var restClient = new RestClient(_postMessageUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {_authKey}");

            // note: RestSharp does not support renaming
            var messageJson = JsonConvert.SerializeObject(message);
            request.AddJsonBody(messageJson, "application/json");

            var response = restClient.Execute<MessageDTO>(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;

            throw GenerateWrongResponseException(response);
        }

        private WebException GenerateWrongResponseException(IRestResponse response)
        {
            return new WebException($"Air Table Response is not OK! Status: {response.StatusCode}, Description: {response.StatusDescription}");
        }
    }
}
