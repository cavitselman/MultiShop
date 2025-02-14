using Microsoft.AspNetCore.Mvc;
using MS.RapidApiWebUI.Models;
using Newtonsoft.Json;

namespace MS.RapidApiWebUI.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<IActionResult> WeatherDetail()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weather-api138.p.rapidapi.com/weather?city_name=manisa"),
                Headers =
    {
        { "x-rapidapi-key", "023a19d0eamsh3c7d7f373f495cdp1ba47fjsn53fe86f6c56a" },
        { "x-rapidapi-host", "weather-api138.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<WeatherViewModel.Rootobject>(body);
                ViewBag.cityTemp = values.main.temp;
                return View();
            }
        }

        public async Task<IActionResult> Exchange()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol=USD&to_symbol=TRY&language=en"),
                Headers =
    {
        { "x-rapidapi-key", "023a19d0eamsh3c7d7f373f495cdp1ba47fjsn53fe86f6c56a" },
        { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ExchangeViewModel.Rootobject>(body);
                ViewBag.exchangeRateUsd = values.data.exchange_rate;
                ViewBag.previous_closeUsd = values.data.previous_close;                
            }

            var client2 = new HttpClient();
            var request2 = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-finance-data.p.rapidapi.com/currency-exchange-rate?from_symbol=EUR&to_symbol=TRY&language=en"),
                Headers =
    {
        { "x-rapidapi-key", "023a19d0eamsh3c7d7f373f495cdp1ba47fjsn53fe86f6c56a" },
        { "x-rapidapi-host", "real-time-finance-data.p.rapidapi.com" },
    },
            };
            using (var response = await client2.SendAsync(request2))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ExchangeViewModel.Rootobject>(body);
                ViewBag.exchangeRateEur = values.data.exchange_rate;
                ViewBag.previous_closeEur = values.data.previous_close;
                return View();
            }
        }
    }
}
