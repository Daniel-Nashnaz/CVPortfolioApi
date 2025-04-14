using GitHubServiceLib.Interfaces;
using GitHubServiceLib.Services.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubServiceLib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGitHubServices(this IServiceCollection services)
        {
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddScoped<IGitHubService, GitHubService>();

            services.Decorate<IGitHubService, CachedGitHubService>();

            return services;
        }
    }

}
