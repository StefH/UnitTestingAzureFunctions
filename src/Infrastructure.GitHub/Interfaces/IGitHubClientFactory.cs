using Infrastructure.GitHub.Interfaces.Public;

namespace Infrastructure.GitHub.Interfaces
{
    /// <summary>
    /// A factory to create an GitHub Client.
    /// </summary>
    public interface IGitHubClientFactory
    {
        IGitHubApi CreateClient();
    }
}