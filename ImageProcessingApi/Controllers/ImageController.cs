using ImageProcessingApi.Logic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ImageFileHandler _imageFileHandler;
        private readonly IWebHostEnvironment _env;

        public ImageController(ILogger<ImageController> logger, ImageFileHandler imageFileHandler, IWebHostEnvironment env)
        {
            _logger = logger;
            _imageFileHandler = imageFileHandler;
            _env = env;
        }


        [HttpPost("process")]
        public async Task<IActionResult> Process(/*[FromForm]*/ IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var inputPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            var id = Guid.NewGuid().ToString();

            await _imageFileHandler.ProcessImage(file, inputPath, id);

            var downloadUrl = Url.Action(
                nameof(Download),            
                "Image",                    
                new { id },                  
                Request.Scheme                
            );

            return Ok(new { downloadUrl });
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var path = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", $"{id}.png");
            if (!System.IO.File.Exists(path))
                return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(bytes, "image/png", $"{id}.png");
        }

        //todo using problems details
    }
}
