using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace CSharpOddOrEven
{
    public static class OddOrEvenHttpTrigger
    {
        [FunctionName("OddOrEvenHttpTrigger")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")]HttpRequest req, ILogger logger)
        {
            logger.LogInformation("Odd or even trigger fired - HTTP");

            string numberQueryValue = req.Query["number"];

            if (BigInteger.TryParse(numberQueryValue, out BigInteger number))
            {
                return new OkObjectResult(number % 2 == 0 ? "Even" : "Odd");
            }
            else
            {
                string message = $"Unable to parse the query parameter 'number'. Got value: {numberQueryValue}";
                logger.LogError(message);

                return new BadRequestObjectResult(message);
            }
        }
    }
}