using ImageProcessor;

namespace ImageProcessingApi.Logic
{
    /// <summary>
    /// The high level business logic behind the rest API.
    /// </summary>
    public class ImageFileProcessor
    {
        /// <summary>
        /// Asyncronuously processes the image and returns it as bytes. Returns an empty array if the provided image is invalid.
        /// </summary>
        public async Task<byte[]> ProcessImage(IFormFile image, EncodingType encodingType, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream, cancellationToken);
                var imageBytes = stream.ToArray();

                return await Task.Run(() => Processor.Process(imageBytes, encodingType.GetFileExtension()), cancellationToken);
            }
        }
    }
}
