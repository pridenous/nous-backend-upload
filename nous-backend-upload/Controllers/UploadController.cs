using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nous_backend_upload.Core;
using nous_backend_upload.DTO;
using nous_backend_upload.Services;

namespace nous_backend_upload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        [HttpGet("GetListFiles")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = await _uploadService.GetListFiles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("UploadFiles")]
        public async Task<ActionResult> Upload([FromBody] RequestUpload request)
        {
            try
            {
                var result = await _uploadService.UploadFiles(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("DeleteFiles")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var result = await _uploadService.DeleteFiles(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("EditFiles")]
        public async Task<ActionResult> Edit([FromBody] RequestEdit request)
        {
            try
            {
                var result = await _uploadService.EditFiles(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
