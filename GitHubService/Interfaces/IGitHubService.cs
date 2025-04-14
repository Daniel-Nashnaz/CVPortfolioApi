using GitHubServiceLib.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubServiceLib.Interfaces
{
    public interface IGitHubService
    {
        Task<IEnumerable<RepositoryInfo>> GetPortfolio();
        Task<IEnumerable<RepositoryInfo>> SearchRepositories(string repoName = null, Language? language = null, string username = null);
        Task<DateTime?> GetLastActivityDate();
    }
}
