using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using FunctionsV2DiSample.FunctionApp.Configs;
using FunctionsV2DiSample.FunctionApp.Functions.FunctionOptions;

using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionsV2DiSample.FunctionApp.Functions
{
    /// <summary>
    /// This represents the function entity for GitHub repositories.
    /// </summary>
    public class CoreGitHubRepositoriesFunction : IGitHubRepositoriesFunction
    {
        private static MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json");
        private static ProductInfoHeaderValue userAgentHeader = new ProductInfoHeaderValue("Mozilla", "5.0");

        private readonly GitHubSettings _github;
        private readonly HttpClient _httpClient;

        public CoreGitHubRepositoriesFunction(GitHubSettings github, HttpClient httpClient)
        {
            _github = github;
            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public ILogger Log { get; set; }

        /// <inheritdoc />
        public async Task<TOutput> InvokeAsync<TInput, TOutput>(TInput input, FunctionOptionsBase options)
        {
            var option = options as GitHubRepositoriesHttpTriggerOptions;

            string result;

            AddRequestHeaders();

            string requestUrl = $"{_github.BaseUrl}{string.Format(_github.Endpoints.Repositories, option.Type, option.Name)}";
            using (var message = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false))
            {
                result = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            var res = JsonConvert.DeserializeObject<object>(result);

            RemoveRequestHeaders();

            return (TOutput)res;
        }

        private void AddRequestHeaders()
        {
            // https://gist.github.com/BellaCode/c0ba0a842bbe22c9215e
            _httpClient.DefaultRequestHeaders.Accept.Add(acceptHeader);
            _httpClient.DefaultRequestHeaders.UserAgent.Add(userAgentHeader);
        }

        private void RemoveRequestHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Remove(acceptHeader);
            _httpClient.DefaultRequestHeaders.UserAgent.Remove(userAgentHeader);
        }
    }
}