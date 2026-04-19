using Api.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudStorageController : ControllerBase
    {
        private GcsService _gcsService = new GcsService();
        [HttpGet("{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            var data = await _gcsService.DownloadFileAsync(fileName);
            return File(data, "application/octet-stream", fileName);
        }

    }
}
