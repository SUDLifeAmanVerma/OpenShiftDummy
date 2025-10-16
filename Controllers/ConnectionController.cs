using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConnectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetConnectionString()
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            return Ok(connStr);
        }
    }
}
