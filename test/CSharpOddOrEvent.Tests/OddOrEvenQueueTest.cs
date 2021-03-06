using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using CSharpOddOrEvenHttpTrigger;
using Xunit;
using Xunit.Abstractions;

namespace CSharpOddOrEven.Tests
{
    public class OddOrEvenQueueTest
    {
        private readonly ITestOutputHelper output;
        private readonly Mock<MockHttpMessageHandler> mockHttpMessageHandler;
        private HttpRequestMessage request;

        public OddOrEvenQueueTest(ITestOutputHelper output)
        {
            mockHttpMessageHandler = new Mock<MockHttpMessageHandler> { CallBase = true };
            this.output = output;

            mockHttpMessageHandler
                .Setup(
                    m => m.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = null
                    })
                .Callback<HttpRequestMessage>(request => this.request = request);

            OddOrEvenQueue.client = new HttpClient(mockHttpMessageHandler.Object);
        }

        [Theory]
        [MemberData(nameof(Numbers.EvenNumbers), MemberType = typeof(Numbers))]
        public async Task EvenNumberAsync(BigInteger number)
        {
            FunctionTestLogger logger = new FunctionTestLogger(output);

            await OddOrEvenQueue.RunAsync(number.ToString(), logger);

            bool wasEven = (from l in logger.getLogs()
                            where l.Equals("Was even")
                            select l).Any();

            Assert.True(wasEven);

            Assert.Equal("Even", await request.Content.ReadAsStringAsync());
        }

        [Theory]
        [MemberData(nameof(Numbers.OddNumbers), MemberType = typeof(Numbers))]
        public async Task OddNumberAsync(BigInteger number)
        {
            FunctionTestLogger logger = new FunctionTestLogger(output);

            await OddOrEvenQueue.RunAsync(number.ToString(), logger);

            bool wasOdd = (from l in logger.getLogs()
                           where l.Equals("Was odd")
                           select l).Any();

            Assert.True(wasOdd);

            Assert.Equal("Odd", await request.Content.ReadAsStringAsync());
        }

        [Fact]
        public void NonNumbers()
        {
            string nonNumber = "I'm Even";
            FunctionTestLogger logger = new FunctionTestLogger(output);

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await OddOrEvenQueue.RunAsync(nonNumber, logger)
            );
        }
    }
}
