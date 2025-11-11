using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    /// <summary>
    /// processálás, fájl kezelés
    /// </summary>
    public class ImageFileProcessor
    {
        public async Task ProcessImage(IFormFile image, string inputPath, string id, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(inputPath);
            var outputPath = Path.Combine(inputPath, $"{id}.jpg");

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream, cancellationToken);
                var imageBytes = stream.ToArray();

                var processedImageBytes = await Task.Run(() => Processor.Process(imageBytes), cancellationToken);
                await System.IO.File.WriteAllBytesAsync(outputPath, processedImageBytes, cancellationToken);  //todo using
            }
        }
    }
}
