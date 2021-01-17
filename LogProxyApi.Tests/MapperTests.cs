using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace LogProxyApi.UnitTests
{
    class MapperTests
    {
        IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new LogProxyApi.Entities.LogMapperProfile()));
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public void ValidateConfiguration()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
