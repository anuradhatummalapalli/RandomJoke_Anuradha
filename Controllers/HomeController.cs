using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RandomJoke_Anuradha.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace RandomJoke_Anuradha.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<object> GetRamdomJoke(int count)
        {
            using (HttpClient client = new HttpClient())
            {

                try
                {
                    string path = string.Empty;
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "df42bff5fbmsh7167692fba0a119p187c16jsn978d5760b93f");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "dad-jokes.p.rapidapi.com");
                    client.BaseAddress = new Uri("https://dad-jokes.p.rapidapi.com/random/");
                    if (count == 0)
                    {
                        path = "joke";
                    }
                    else
                    {
                        path = "joke?count=" + count;
                    }

                    var response = client.GetAsync(path);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonResult = result.Content.ReadAsStringAsync().Result;
                        return Json(JsonConvert.SerializeObject(jsonResult));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong: {ex}");
                    return StatusCode(500, "Internal server error");
                }
                return null;
            }
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}