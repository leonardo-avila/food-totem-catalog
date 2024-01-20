using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.Catalog.API.Controllers
{
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult HealthCheck()
        {
            return Ok("API is up and running!");
        }
    }
}