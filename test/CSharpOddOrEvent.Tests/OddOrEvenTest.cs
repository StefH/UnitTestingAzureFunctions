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
using CSharpOddOrEvenHttpTrigger;

namespace CSharpOddOrEven.Tests
{
    public class OddOrEvenTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<HttpRequest> _httpRequestMock;
        private readonly Mock<ITransientGreeter> _greeterTransientMock;
        private readonly Mock<IScopedGreeter> _greeterScopedMock;
        private readonly Mock<ISingletonGreeter> _greeterSingletonMock;

        public OddOrEvenTests()
        {
            _loggerMock = new Mock<ILogger>();
            _loggerMock.SetupAllProperties();
            _loggerMock.Setup(l => l.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));

            _httpRequestMock = new Mock<HttpRequest>();
            _httpRequestMock.SetupAllProperties();

            _greeterSingletonMock = new Mock<ISingletonGreeter>();
            _greeterSingletonMock.SetupAllProperties();

            _greeterTransientMock = new Mock<ITransientGreeter>();
            _greeterTransientMock.SetupAllProperties();

            _greeterScopedMock = new Mock<IScopedGreeter>();
            _greeterScopedMock.SetupAllProperties();
        }

        [Theory]
        [MemberData(nameof(Numbers.EvenNumbers), MemberType = typeof(Numbers))]
        public void EvenNumber(BigInteger number)
        {
            // Assign
            _httpRequestMock.SetupGetQuery(new Dictionary<string, StringValues>() { { "number", number.ToString() } });

            // Act
            var response = OddOrEvenHttpTrigger.Run(
                _httpRequestMock.Object,
                _greeterTransientMock.Object,
                _greeterTransientMock.Object,
                _greeterScopedMock.Object,
                _greeterScopedMock.Object,
                _greeterSingletonMock.Object,
                _greeterSingletonMock.Object,
                _loggerMock.Object);

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
            var response = OddOrEvenHttpTrigger.Run(
                _httpRequestMock.Object,
                _greeterTransientMock.Object,
                _greeterTransientMock.Object,
                _greeterScopedMock.Object,
                _greeterScopedMock.Object,
                _greeterSingletonMock.Object,
                _greeterSingletonMock.Object,
                _loggerMock.Object);

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
            var response = OddOrEvenHttpTrigger.Run(
                _httpRequestMock.Object,
                _greeterTransientMock.Object,
                _greeterTransientMock.Object,
                _greeterScopedMock.Object,
                _greeterScopedMock.Object,
                _greeterSingletonMock.Object,
                _greeterSingletonMock.Object,
                _loggerMock.Object);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response);
            Assert.Contains("Unable to parse", ((BadRequestObjectResult)response).Value as string);

            // Verify
            _loggerMock.Verify(l => l.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
            _loggerMock.Verify(l => l.Log(LogLevel.Error, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }
    }
}
