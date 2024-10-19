using ManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpPost("module/health")]
        public IActionResult PostHealthCheck([FromBody] HealthCheckModel model)
        {
            Log.Information("Received HealthCheck: {Id}, {SystemTime}, {NumberofConnectedClients}",
                            model.Id, model.SystemTime, model.NumberofConnectedClients);

            // ساخت پاسخ برای HealthCheck
            var response = new HealthCheckResponse
            {
                IsEnabled = true,
                NumberOfActiveClients = new Random().Next(0, 6), // عدد تصادفی بین 0 تا 5
                ExpirationTime = DateTime.Now.AddMinutes(10) // زمان انقضا برابر با 10 دقیقه بعد
            };

            return Ok(response);
        }
    }
}
