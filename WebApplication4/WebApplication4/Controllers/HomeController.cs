using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // Use HttpClient to fetch the HTML
            using (var client = new HttpClient())
            {
                string targetUrl = "https://amovee2014.com/category/%d7%91%d7%a2%d7%9c%d7%99-%d7%9e%d7%a7%d7%a6%d7%95%d7%a2-%d7%9c%d7%91%d7%99%d7%aa//"; // Replace with the desired URL
                string htmlContent = await client.GetStringAsync(targetUrl);
                var doc = new HtmlDocument();

                doc.LoadHtml(htmlContent);

                var aList = doc.DocumentNode.Descendants("a");
                var divList = doc.DocumentNode.Descendants("div");
                var titles =aList.Where(a=>a.HasClass("blog_block__body-content-title")).ToList();
                var contents =divList.Where(a=>a.HasClass("blog_block__body-content-excerpt")).ToList();
                var links = aList.Where(a=>a.HasClass("blog_block__body-content-readmore")).ToList();
                List<NewsData> newsDatas = new List<NewsData>();
                for (int i = 0; i < titles.Count(); i++)
                {
                   // newsDatas.Add(new NewsData(){ Title = titles[i].InnerText, Contnet = "abc"+i, Link = links[i].Attributes["href"].Value});
               //     newsDatas.Add(new NewsData(){ Index = i, Title = "title"+i, Body = "title"+i, Link = links[i].Attributes["href"].Value});
                    newsDatas.Add(new NewsData(){ Index = i, Title = titles[i].InnerText, Body = contents[i].InnerText, Link = links[i].Attributes["href"].Value});
                }

                ViewData["Data"] = newsDatas;
                // Pass the fetched HTML to the view
                return View();
            }
        }
        catch (Exception ex)
        {
            // Handle potential errors gracefully
            return View("Error", ex.Message); // Pass error message to an error view
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}