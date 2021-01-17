using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogProxyApi.Entities
{
    /// <summary>
    /// Profile for mapping log entites
    /// </summary>
    public class LogMapperProfile : Profile
    {
        /// <summary>
        /// Constructor of LogMapperProfile
        /// </summary>
        public LogMapperProfile()
        {
            CreateMap<Message, MessageDTO>()
                 .ForMember(destination => destination.Records, map => map.MapFrom(
                     source => new List<Record>
                     {
                          new Record {
                          Fields = new Fields
                          {
                              Id = Startup.RecordId.ToString(),
                              Message = source.Text,
                              Summary = source.Title,
                              ReceivedAt = DateTimeOffset.Now,
                          }
                          }
                     }));

            CreateMap<MessageDTO, IEnumerable<Message>>()
                .ConvertUsing(source => new List<Message>(
                    source.Records.Select(r => new Message()
                    {
                        Id = r.Fields.Id,
                        ReceivedAt = r.Fields.ReceivedAt,
                        Text = r.Fields.Message,
                        Title = r.Fields.Summary,
                    })
                    .ToList()));
        }
    }
}
