using Infrastructure.GitHub.Interfaces;
using Infrastructure.GitHub.Interfaces.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;

namespace Infrastructure.GitHub.Implementations
{
    /// <seealso cref="IGitHubClientFactory" />
    internal class GitHubClientFactory : IGitHubClientFactory
    {
        /// <summary>
        /// The CamelCasePropertyNamesContractResolver is required for Auth0.
        /// </summary>
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly IHttpClientFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubClientFactory"/> class.
        /// </summary>
        /// <param name="factory">The HttpClientFactory.</param>
        public GitHubClientFactory(IHttpClientFactory factory)
        {
            // Check.NotNull(factory, nameof(factory));

            _factory = factory;
        }

        /// <summary>
        /// Creates the Rest Client.
        /// </summary>
        /// <returns>IGitHubApi</returns>
        public IGitHubApi CreateClient()
        {
            var client = new RestClient(_factory.GetHttpClient())
            {
                JsonSerializerSettings = _serializerSettings
            }.For<IGitHubApi>();

            client.Api = "https://api.github.com";

            return client;
        }
    }
}