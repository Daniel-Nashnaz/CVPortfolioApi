using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using GitHubServiceLib.Models;
using Microsoft.Extensions.Options;
using GitHubServiceLib.Interfaces;
using GitHubServiceLib.Options;

namespace GitHubServiceLib
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private string _username;

        // Initializes GitHub client with authentication token
        public GitHubService(IOptions<GitHubOptions> options)
        {
            _client = new GitHubClient(new ProductHeaderValue("CVPortfolioAPI"));

            if (!string.IsNullOrEmpty(options.Value.PersonalAccessToken))
            {
                _client.Credentials = new Credentials(options.Value.PersonalAccessToken);
            }
        }

        // Get current authenticated user's username if not already loaded
        private async Task EnsureUsername()
        {
            if (string.IsNullOrEmpty(_username))
            {
                var user = await _client.User.Current();
                _username = user.Login;
            }
        }

        // Retrieves the user's repositories with detailed information
        public async Task<IEnumerable<RepositoryInfo>> GetPortfolio()
        {
            await EnsureUsername();

            var repos = await _client.Repository.GetAllForUser(_username);
            var result = new List<RepositoryInfo>();

            foreach (var repo in repos)
            {
                // For each repo, gather additional information
                var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
               
                result.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    Url = repo.HtmlUrl,
                    WebsiteUrl = repo.Homepage,
                    LastCommitDate = repo.UpdatedAt.UtcDateTime,
                    Stars = repo.StargazersCount,
                    PullRequests = pullRequests.Count,
                    Languages = languages.Select(l => l.Name).ToList()
                });
            }

            return result;
        }

        // Search for public repositories based on optional criteria
        public async Task<IEnumerable<RepositoryInfo>> SearchRepositories(string repoName = null, Language? language = null, string username = null)
        {
            SearchRepositoriesRequest request = null;
            if (!string.IsNullOrEmpty(repoName))
            {
                request = new SearchRepositoriesRequest(repoName);
            }
            else
            {
                request = new SearchRepositoriesRequest();

            }

            if (language.HasValue)
                request.Language = language.Value;

            if (!string.IsNullOrEmpty(username))
                request.User = username;

            var searchResult = await _client.Search.SearchRepo(request);
            var result = new List<RepositoryInfo>();

            foreach (var repo in searchResult.Items)
            {
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);

                result.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    Url = repo.HtmlUrl,
                    WebsiteUrl = repo.Homepage,
                    LastCommitDate = repo.UpdatedAt.UtcDateTime,
                    Stars = repo.StargazersCount,
                    Languages = languages.Select(l => l.Name).ToList()
                });
            }

            return result;
        }

        // Gets the timestamp of the latest activity for the authenticated user
        public async Task<DateTime?> GetLastActivityDate()
        {
            await EnsureUsername();
            var events = await _client.Activity.Events.GetAllUserPerformed(_username);
            return events.FirstOrDefault()?.CreatedAt.UtcDateTime;
        }
    }
}
