using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConnectionController> _logger;

        
        public ConnectionController(IConfiguration configuration, ILogger<ConnectionController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetConnectionString()
        {
            _logger.LogInformation("Connection String Contoller");
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            return Ok(connStr);
        }
    }
}
