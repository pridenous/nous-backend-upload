using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nous_backend_upload.DTO;
using nous_backend_upload.Services;

namespace nous_backend_upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("GoogleLogin")]
        public async Task<ActionResult> GoogleLogin([FromBody] GoogleRequestLogin request)
        {
            try
            {
                var result = await _loginService.ProcessLoginByGoogle(request.access_token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
