using System.Text.Json;
using homework19.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace homework19.Controllers

public class WeatherController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private const string ApiKey = "fbb611c0340b78c89b32347005fe2540";

    public WeatherController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ViewBag.Error = "Пожалуйста, введите название города.";
            return View();
        }

        var client = _clientFactory.CreateClient();
        
        // units=metric - температура в градусах Цельсия,
        // lang=ru - русский язык описания погоды
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&units=metric&lang=ru&appid={ApiKey}";

        try
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var document = await JsonDocument.ParseAsync(responseStream);
                var root = document.RootElement;
                
                int cod = 0;
                if (root.TryGetProperty("cod", out JsonElement codElement))
                {
                    if (codElement.ValueKind == JsonValueKind.Number)
                    {
                        cod = codElement.GetInt32();
                    }
                    else if (codElement.ValueKind == JsonValueKind.String && int.TryParse(codElement.GetString(), out int parsedCod))
                    {
                        cod = parsedCod;
                    }
                }
                if (cod != 200)
                {
                    string message = root.GetProperty("message").GetString();
                    ViewBag.Error = $"Ошибка: {message}";
                    return View();
                }
                
                var weatherData = new WeatherViewModel
                {
                    City = root.GetProperty("name").GetString(),
                    Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                    FeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                    Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                    Description = root.GetProperty("weather")[0].GetProperty("description").GetString()
                };

                return View(weatherData);
            }
            else
            {
                ViewBag.Error = $"Ошибка: {response.StatusCode}";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Ошибка: {ex.Message}";
            return View();
        }
    }
}

