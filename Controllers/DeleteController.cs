using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeleteController> _logger;


        public DeleteController(IConfiguration configuration, ILogger<DeleteController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _logger.LogInformation("Delete Controller.");
            string query = "DELETE FROM Products WHERE Id = @Id";
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            return Ok("Product deleted successfully.");
        }
    }
}
