using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OpenShiftDummy.Models;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReadController> _logger;


        public ReadController(IConfiguration configuration, ILogger<ReadController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            _logger.LogInformation("Read Controller");
            List<Product> products = new();
            string query = "SELECT * FROM Products";
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }
            return Ok(products);
        }
    }
}
