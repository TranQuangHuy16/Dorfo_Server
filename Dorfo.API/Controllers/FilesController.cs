using Dorfo.Application.DTOs.Requests;
using Dorfo.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly CloudinaryStorageHelper _helper;

        public FilesController(CloudinaryStorageHelper helper)
        {
            _helper = helper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto file)
        {
            try
            {
                var url = await _helper.UploadFileAsync(file.File);
                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
