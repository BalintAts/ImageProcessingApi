using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    /// <summary>
    /// processálás, fájl kezelés
    /// </summary>
    public class ImageFileProcessor
    {
        public async Task ProcessImage(IFormFile image, string inputPath, EncodingType encodingType, string id, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(inputPath);
            var outputPath = Path.Combine(inputPath, id);

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream, cancellationToken);
                var imageBytes = stream.ToArray();

                var processedImageBytes = await Task.Run(() => Processor.Process(imageBytes, encodingType.GetExtension()), cancellationToken);
                await System.IO.File.WriteAllBytesAsync(outputPath, processedImageBytes, cancellationToken);  //todo using
            }
        }

        public async Task<byte[]> ProcessImage2(IFormFile image, string inputPath, EncodingType encodingType, string id, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(inputPath);
            var outputPath = Path.Combine(inputPath, id);

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream, cancellationToken);
                var imageBytes = stream.ToArray();

                return await Task.Run(() => Processor.Process(imageBytes, encodingType.GetExtension()), cancellationToken);
            }

        }
    }
}
