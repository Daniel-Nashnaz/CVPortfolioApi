using GitHubServiceLib.Interfaces;
using GitHubServiceLib.Services.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubServiceLib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extension method to register all GitHub-related services
        public static IServiceCollection AddGitHubServices(this IServiceCollection services)
        {
            // Register memory cache as singleton (one instance for the entire application)
            services.AddSingleton<IMemoryCache, MemoryCache>();

            // Register GitHub service with scoped lifetime (one instance per HTTP request)
            services.AddScoped<IGitHubService, GitHubService>();

            // Apply the caching decorator to the GitHub service
            services.Decorate<IGitHubService, CachedGitHubService>();

            return services;
        }
    }

}
