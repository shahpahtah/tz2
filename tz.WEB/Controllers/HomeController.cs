using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using tz.WEB.Models;


namespace tz.WEB.Controllers
{
    [ApiController]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("Response")]
        public async Task<IActionResult> Response([FromBody] User user)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "RLPUUOQAMIKSAB2PSGUECA");

            var body = new
            {
                stream_code = "vv4uf",
                client = new
                {
                    phone = user.cellphone,
                    name = user.name
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("https://order.drcash.sh/v1/order", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("—пасибо за заказ.");
                }
                else
                {
                    return BadRequest("ќшибка при обработке заказа.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ќшибка при выполнении запроса к API.");
                return StatusCode(500, "¬нутренн€€ ошибка сервера.");
            }
        }

       
    }
}