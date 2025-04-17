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

        //Accepts the original service and cache manager
        public CachedGitHubService(IGitHubService gitHubService, IMemoryCache cache)
        {
            _gitHubService = gitHubService;
            _cache = cache;
        }

        // Gets portfolio data with caching support
        public async Task<IEnumerable<RepositoryInfo>> GetPortfolio()
        {
            // Try to get data from cache
            if (_cache.TryGetValue(PortfolioCacheKey, out IEnumerable<RepositoryInfo> cachedPortfolio))
            {
                // Check if user had any new activity since last cache
                var lastActivity = await GetLastActivityDate();
                var cachedLastActivity = _cache.Get<DateTime?>(LastActivityCacheKey);

                if (cachedLastActivity.HasValue && lastActivity.HasValue && lastActivity > cachedLastActivity)
                {
                    // If newer activity exists, invalidate cache
                    _cache.Remove(PortfolioCacheKey);
                }
                else
                {
                    // Return cached data if still valid
                    return cachedPortfolio;
                }
            }

            // Get fresh data if cache is empty or invalid
            var portfolio = await _gitHubService.GetPortfolio();
            var currentLastActivity = await GetLastActivityDate();

            // Save to cache with expiration
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