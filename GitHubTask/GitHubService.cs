using GitHubTask.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GitHubTask
{
    public class GitHubService
    {

        private readonly HttpClient _http;

        public GitHubService(HttpClient httpClient)
        {
            _http = httpClient;
            _http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
        }

        public async Task<List<GitHub>> GetReposAsync(int page, int pageSize = 10)
        {
            var since = DateTime.UtcNow.AddDays(-10).ToString("yyyy-MM-dd");
            var url = $"https://api.github.com/search/repositories?q=created:>{since}&sort=stars&order=desc&page={page}&per_page={pageSize}";

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var items = doc.RootElement.GetProperty("items");

            var repos = new List<GitHub>();

            foreach (var item in items.EnumerateArray())
            {
                repos.Add(new GitHub
                {
                    Name = item.GetProperty("name").GetString(),
                    Description = item.GetProperty("description").GetString(),
                    Stars = item.GetProperty("stargazers_count").GetInt32(),
                    OwnerName = item.GetProperty("owner").GetProperty("login").GetString(),
                    OwnerAvatar = item.GetProperty("owner").GetProperty("avatar_url").GetString()
                });
            }

            return repos;
        }
    }
}

