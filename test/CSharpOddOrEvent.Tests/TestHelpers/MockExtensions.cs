using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Moq;

namespace CSharpOddOrEven.Tests.TestHelpers
{
    public static class MockExtensions
    {
        public static void SetupGetQuery(this Mock<HttpRequest> requestMock, Dictionary<string, StringValues> queryParams)
        {
            requestMock.SetupGet(h => h.Query).Returns(new QueryCollection(queryParams));
        }
    }
}