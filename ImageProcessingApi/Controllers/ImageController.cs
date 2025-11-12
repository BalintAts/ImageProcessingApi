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
        private readonly ImageFileProcessor _imageFileHandler;
        private readonly IWebHostEnvironment _env;

        public ImageController(ILogger<ImageController> logger, ImageFileProcessor imageFileHandler, IWebHostEnvironment env)
        {
            _logger = logger;
            _imageFileHandler = imageFileHandler;
            _env = env;
        }

        //todo using problems details

        [HttpPost("process")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Process(IFormFile file, [FromForm] EncodingType encodingType, CancellationToken cancellationToken)
        {
            //var result = await _imageFileHandler.ProcessImage(file, encodingType, cancellationToken);


            throw new Exception("alma, körte , szőlő");
                return Problem(
                    title: "User not found",
                    statusCode: 404,
                    detail: $"User with ID 1 does not exist.",
                    type: "https://example.com/problems/user-not-found",
                    instance: $"/users/1"
                );
            
            //return new FileStreamResult(new MemoryStream(result), "image/png");
        }

        //[ApiExplorerSettings(IgnoreApi = true)]
        //[Route("/error")]
        //public IActionResult HandleError()
        //{
        //    return Problem();
        //}
    }
}
// megh: input:  binary string,  jpgbe konvertál mindig   
