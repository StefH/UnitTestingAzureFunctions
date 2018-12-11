using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using CSharpOddOrEvenHttpTrigger;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace CSharpOddOrEven
{
    public static class OddOrEvenHttpTrigger
    {
        [FunctionName("OddOrEvenHttpTrigger")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]HttpRequest req,
            [Inject]ITransientGreeter transientGreeter1,
            [Inject]ITransientGreeter transientGreeter2,
            [Inject]IScopedGreeter scopedGreeter1,
            [Inject]IScopedGreeter scopedGreeter2,
            [Inject]ISingletonGreeter singletonGreeter1,
            [Inject]ISingletonGreeter singletonGreeter2,
            ILogger logger)
        {
            logger.LogInformation("Odd or even trigger fired - HTTP");

            string numberQueryValue = req.Query["number"];

            if (BigInteger.TryParse(numberQueryValue, out BigInteger number))
            {
                var result = string.Join(Environment.NewLine, new[] {
                    $"Transient: {transientGreeter1.Greet()}",
                    $"Transient: {transientGreeter2.Greet()}",
                    $"Scoped: {scopedGreeter1.Greet()}",
                    $"Scoped: {scopedGreeter2.Greet()}",
                    $"Singleton: {singletonGreeter1.Greet()}",
                    $"Singleton: {singletonGreeter2.Greet()}"
                });
                logger.LogWarning(result);

                string num = number % 2 == 0 ? "Even" : "Odd";

                return new OkObjectResult(num);
            }
            else
            {
                string message = $"Unable to parse the query parameter 'number'. Got value: {numberQueryValue}";
                logger.LogError(message);

                return new BadRequestObjectResult(message);
            }
        }

        [FunctionName("LoggingGreeter")]
        public static IActionResult RunLoggingGreeter(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req,
            [Inject]LoggingGreeter greeter)
        {
            greeter.Greet();
            return new OkResult();
        }

        public static string GetEnvironmentVariable(string name)
        {
            return name + ": " + System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}