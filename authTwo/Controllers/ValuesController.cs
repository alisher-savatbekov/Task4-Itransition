using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authTwo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ValuesController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("check")]
        public IActionResult Get()
        {
            return Ok("+++");
        }
    }
}
