using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GitHubTask.Models;
using static GitHubTask.GitHubService;


namespace GitHubTask.Controllers;

public class HomeController : Controller
{
    private readonly GitHubService _githubService;
    public HomeController(GitHubService githubService)
    {
        _githubService = githubService;
    }

    public async Task<IActionResult> Index()
    {

        var repos = await  _githubService.GetReposAsync(1);
        return View(repos);
    }
    public async Task<IActionResult> LoadMore(int page )
    {
        var repos = await _githubService.GetReposAsync(page);
        return PartialView("PartialList", repos);
    }
}
