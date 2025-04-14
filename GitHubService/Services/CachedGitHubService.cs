using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubServiceLib.Interfaces;
using GitHubServiceLib.Models;
using Microsoft.Extensions.Caching.Memory;
using Octokit;

namespace GitHubServiceLib.Services.Services
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);
        private const string PortfolioCacheKey = "portfolio";
        private const string LastActivityCacheKey = "lastActivity";

        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache cache)
        {
            _gitHubService = gitHubService;
            _cache = cache;
        }

        public async Task<IEnumerable<RepositoryInfo>> GetPortfolio()
        {
            if (_cache.TryGetValue(PortfolioCacheKey, out IEnumerable<RepositoryInfo> cachedPortfolio))
            {
                var lastActivity = await GetLastActivityDate();
                var cachedLastActivity = _cache.Get<DateTime?>(LastActivityCacheKey);

                if (cachedLastActivity.HasValue && lastActivity.HasValue && lastActivity > cachedLastActivity)
                {
                    _cache.Remove(PortfolioCacheKey);
                }
                else
                {
                    return cachedPortfolio;
                }
            }

            var portfolio = await _gitHubService.GetPortfolio();
            var currentLastActivity = await GetLastActivityDate();

            _cache.Set(PortfolioCacheKey, portfolio, _cacheExpiration);
            _cache.Set(LastActivityCacheKey, currentLastActivity, _cacheExpiration);

            return portfolio;
        }

        public async Task<IEnumerable<RepositoryInfo>> SearchRepositories(string repoName = null, Language? language = null, string username = null)
        {
            return await _gitHubService.SearchRepositories(repoName, language, username);
        }

        public async Task<DateTime?> GetLastActivityDate()
        {
            return await _gitHubService.GetLastActivityDate();
        }
    }
}