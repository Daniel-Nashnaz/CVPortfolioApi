using System;
using System.Collections.Generic;

namespace GitHubServiceLib.Models
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime LastCommitDate { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
    }
}
