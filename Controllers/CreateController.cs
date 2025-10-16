using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OpenShiftDummy.Models;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateController> _logger;


        public CreateController(IConfiguration configuration, ILogger<CreateController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            _logger.LogInformation("Create product Controller");
            string query = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            conn.Open();
            cmd.ExecuteNonQuery();
            return Ok("Product created successfully.");
        }
    }
}
