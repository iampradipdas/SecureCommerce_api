using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureCommerce_api.Bal.Interfaces;

namespace SecureCommerce_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IImageService _imageService;

        public MediaController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                var imageUrl = await _imageService.UploadImageAsync(file, "products");
                return Ok(new { ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error during upload.", Details = ex.Message, InnerDetails = ex.InnerException?.Message });
            }
        }
    }
}
