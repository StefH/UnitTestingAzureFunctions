using System.Net.Http;

namespace Infrastructure.GitHub.Interfaces
{
    /// <summary>
    /// Factory to create a HttpClient.
    /// </summary>
    internal interface IHttpClientFactory
    {
        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns></returns>
        HttpClient GetHttpClient();
    }
}