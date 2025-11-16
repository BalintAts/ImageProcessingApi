using ImageProcessingApi.Logic;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageFileProcessor _imageFileHandler;
        private readonly IWebHostEnvironment _env;

        public ImageController(ILogger<ImageController> logger, ImageFileProcessor imageFileHandler, IWebHostEnvironment env)
        {
            _logger = logger;
            _imageFileHandler = imageFileHandler;
            _env = env;
        }

        [HttpPost("process")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Process(IFormFile file, [FromForm] EncodingType encodingType, CancellationToken cancellationToken)
        {
            var result = await _imageFileHandler.ProcessImage(file, encodingType, cancellationToken);
            if (result.Length == 0)
            {
                var errorMessage = "Could not encode the provided image.";
                _logger.LogError(errorMessage);
                return Problem(errorMessage);
            }
            return new FileStreamResult(new MemoryStream(result), encodingType.GetMimeType());
        }
    }
}