using static System.Runtime.InteropServices.JavaScript.JSType;
using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    public class ImageFileHandler
    {
        public async Task ProcessImage(IFormFile image, string inputPath, string id)
        {
            // Save input file temporarily
            //var inputPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
            Directory.CreateDirectory(inputPath);

            //var id = Guid.NewGuid().ToString();
            var outputFile = Path.Combine(inputPath, $"{id}.png");

            // Convert to bytes (if you need to process in memory)
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                var bytes = stream.ToArray();

                // TODO: Process the image bytes here (e.g. call C++ function)
                Processor.Process();
                await System.IO.File.WriteAllBytesAsync(outputFile, bytes);
            }
        }
    }
}
