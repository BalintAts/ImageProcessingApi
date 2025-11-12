using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    /// <summary>
    /// The high level business logic behind the rest API.
    /// </summary>
    public class ImageFileProcessor
    {
        public async Task<byte[]> ProcessImage(IFormFile image, EncodingType encodingType, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream, cancellationToken);
                var imageBytes = stream.ToArray();

                return null;

                return await Task.Run(() => Processor.Process(imageBytes, encodingType.GetExtension()), cancellationToken);
            }
        }
    }
}
