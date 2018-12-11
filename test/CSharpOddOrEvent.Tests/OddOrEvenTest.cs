using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Numerics;
using Moq;
using Xunit;
using System;
using CSharpOddOrEven.Tests.TestHelpers;

namespace CSharpOddOrEven.Tests
{
    public class OddOrEvenTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;

        public OddOrEvenTests()
        {
            _loggerMock = new Mock<ILogger>();
            _loggerMock.SetupAllProperties();
            _loggerMock.Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));

            _httpRequestMock = new Mock<HttpRequest>();
            _httpRequestMock.SetupAllProperties();
        }

        [Theory]
        [MemberData(nameof(Numbers.EvenNumbers), MemberType = typeof(Numbers))]
        public void EvenNumber(BigInteger number)
        {
            // Assign
            _httpRequestMock.SetupGetQuery(new Dictionary<string, StringValues>() { { "number", number.ToString() } });

            // Act
            var response = OddOrEven.Run(_httpRequestMock.Object, _loggerMock.Object);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal("Even", ((OkObjectResult)response).Value as string);

            // Verify
            _loggerMock.Verify(l => l.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
            _loggerMock.Verify(l => l.Log(LogLevel.Error, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(Numbers.OddNumbers), MemberType = typeof(Numbers))]
        public void OddNumber(BigInteger number)
        {
            // Assign
            _httpRequestMock.SetupGetQuery(new Dictionary<string, StringValues>() { { "number", number.ToString() } });

            // Act
            var response = OddOrEven.Run(_httpRequestMock.Object, _loggerMock.Object);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            Assert.Equal("Odd", ((OkObjectResult)response).Value as string);

            // Verify
            _loggerMock.Verify(l => l.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
            _loggerMock.Verify(l => l.Log(LogLevel.Error, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }

        [Fact]
        public void NonNumbers()
        {
            // Assign
            _httpRequestMock.SetupGetQuery(new Dictionary<string, StringValues>() { { "number", "I'm Even" } });

            // Act
            var response = OddOrEven.Run(_httpRequestMock.Object, _loggerMock.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Contains("Unable to parse", ((BadRequestObjectResult)response).Value as string);

            // Verify
            _loggerMock.Verify(l => l.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
            _loggerMock.Verify(l => l.Log(LogLevel.Error, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }
    }
}
