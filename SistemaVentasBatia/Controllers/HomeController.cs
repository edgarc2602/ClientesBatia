using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaClientesBatia.Models;
using System.Diagnostics;

namespace SistemaClientesBatia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
#pragma warning disable IDE0052 // Quitar miembros privados no leídos
        private readonly ILogger<HomeController> _logger;
#pragma warning restore IDE0052 // Quitar miembros privados no leídos

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return new JsonResult(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
