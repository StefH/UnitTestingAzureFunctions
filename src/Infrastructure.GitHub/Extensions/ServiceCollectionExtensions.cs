using Infrastructure.GitHub.Implementations;
using Infrastructure.GitHub.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.GitHub.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGitHub(this IServiceCollection services)
        {
            services.AddServices();
        }

        public static void AddServices(this IServiceCollection services)
        {
            // register the HttpClientFactory and GitHubClientFactory
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddTransient<IGitHubClientFactory, GitHubClientFactory>();
        }
    }
}