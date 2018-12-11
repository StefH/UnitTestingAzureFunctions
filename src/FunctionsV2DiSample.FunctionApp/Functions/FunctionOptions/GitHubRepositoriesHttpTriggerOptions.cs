using Microsoft.AspNetCore.Http;

namespace FunctionsV2DiSample.FunctionApp.Functions.FunctionOptions
{
    /// <summary>
    /// This represents the options entity for the <see cref="CoreGitHubRepositoriesHttpTrigger"/> class.
    /// </summary>
    public class GitHubRepositoriesHttpTriggerOptions : FunctionOptionsBase
    {
        private readonly HttpRequest _req;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubRepositoriesHttpTriggerOptions"/> class.
        /// </summary>
        /// <param name="req"></param>
        public GitHubRepositoriesHttpTriggerOptions(HttpRequest req)
        {
            _req = req;
        }

        /// <summary>
        /// Gets the repository type - users or organisations.
        /// </summary>
        public string Type => GetRepositoryType();

        /// <summary>
        /// Gets the repository name.
        /// </summary>
        public string Name => GetRepositoryName();

        private string GetRepositoryType()
        {
            var type = _req.Query["type"];

            return type;
        }

        private string GetRepositoryName()
        {
            var name = _req.Query["name"];

            return name;
        }
    }
}