using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PS.Notification.Abstractions.Commands;
using System.Threading.Tasks;

namespace PS.Notification.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public HomeController(ILogger<HomeController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Task.FromResult("test"));
        }

        [HttpPost]
        public async Task<ActionResult> Post(string value)
        {
            await _publishEndpoint.Publish(new SendMailCommand { From = "Terter@hotmail.com",  FromDisplayName = "Terterr", To = new string[] { "ertemiz.mustafa@gmail.com" }, Subject = "test", Body = value });

            return Ok();
        }
    }
}
