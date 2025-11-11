using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    public class ImageFileHandler
    {
        public async Task ProcessImage(IFormFile image, string inputPath, string id)
        {
            Directory.CreateDirectory(inputPath);
            var outputFile = Path.Combine(inputPath, $"{id}.png");

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                var imageBytes = stream.ToArray();

                var processedImageBytes = await Task.Run(() => Processor.Process(imageBytes));
                await System.IO.File.WriteAllBytesAsync(outputFile, imageBytes);
            }
        }
    }
}
