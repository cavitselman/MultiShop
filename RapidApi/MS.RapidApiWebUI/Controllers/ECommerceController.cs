using Microsoft.AspNetCore.Mvc;
using MS.RapidApiWebUI.Models;
using Newtonsoft.Json;

namespace MS.RapidApiWebUI.Controllers
{
    public class ECommerceController : Controller
    {
        public async Task<IActionResult> ECommerceList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://real-time-product-search.p.rapidapi.com/search?q=logitech%20fare&country=tr&language=en&page=1&limit=30&sort_by=BEST_MATCH&product_condition=ANY&min_rating=ANY"),
                Headers =
    {
        { "x-rapidapi-key", "023a19d0eamsh3c7d7f373f495cdp1ba47fjsn53fe86f6c56a" },
        { "x-rapidapi-host", "real-time-product-search.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ECommerceViewModel>(body);
                return View(values.data.ToList());
            }
        }
    }
}
