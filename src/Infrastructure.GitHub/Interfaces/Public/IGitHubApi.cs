using System.Threading.Tasks;
using RestEase;

namespace Infrastructure.GitHub.Interfaces.Public
{
    [Header("User-Agent", "Mozilla")]
    [Header("Accept", "application/vnd.github.v3+json")]
    public interface IGitHubApi
    {
        [Path("api", UrlEncode = false)]
        string Api { get; set; }

        // https://api.github.com/users/stefh/repos
        [Get("{api}/users/{userId}/repos")]
        Task<string> GetAsync([Path] string userId);
    }
}