using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OpenShiftDummy.Models;

namespace OpenShiftDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UpdateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            string query = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", product.Id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            conn.Open();
            cmd.ExecuteNonQuery();
            return Ok("Product updated successfully.");
        }
    }
}
