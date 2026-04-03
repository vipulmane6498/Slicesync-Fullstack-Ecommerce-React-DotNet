using Microsoft.AspNetCore.Mvc;

namespace SliceSync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet("abc")]
        public IActionResult NewApi(string ReturnUrl)
        {
            return Ok($"New API: {ReturnUrl}");
        }
    }
}
