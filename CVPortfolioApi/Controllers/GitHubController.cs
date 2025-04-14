using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubServiceLib.Interfaces;
using GitHubServiceLib.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace CVPortfolioApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortfolioController"/> class.
        /// </summary>
        /// <param name="gitHubService">The GitHub service used to retrieve repository data.</param>
        public PortfolioController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        /// <summary>
        /// Retrieves a portfolio of repositories for the configured GitHub user.
        /// </summary>
        /// <returns>A list of repositories with metadata like languages, pull requests</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryInfo>>> GetPortfolio()
        {
            var result = await _gitHubService.GetPortfolio();
            return Ok(result);
        }

        /// <summary>
        /// Searches GitHub repositories by repository name, programming language, or username.
        /// </summary>
        /// <param name="repoName">The name (or partial name) of the repository to search.</param>
        /// <param name="language">The primary language of the repository (from the Octokit.Language enum).</param>
        /// <param name="username">The GitHub username to filter repositories by.</param>
        /// <returns>A filtered list of matching repositories with basic info.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RepositoryInfo>>> SearchRepositories(
            [FromQuery] string repoName = null,
            [FromQuery] Language? language = null,
            [FromQuery] string username = null)
        {
            var result = await _gitHubService.SearchRepositories(repoName, language, username);
            return Ok(result);
        }
    }
}
