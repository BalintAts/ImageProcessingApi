using System.Text.Json.Serialization;

namespace ImageProcessingApi.Logic
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EncodingType
    {
        PNG,
        JPG
    }

    public static class EncodingTypeExtension
    {
        /// <summary>
        /// Gets the file extension of the encoding type (with the ".") 
        /// </summary>
        public static string GetFileExtension(this EncodingType encodingType) => encodingType switch
        {
            EncodingType.PNG => ".png",
            EncodingType.JPG => ".jpg",
            _ => throw new NotImplementedException("No file extension exists for this encoding type."),
        };

        /// <summary>
        /// Gets the MIME type of a file encoded with the gicen encoding type.
        /// </summary>
        public static string GetMimeType(this EncodingType encodingType) => encodingType switch
        {
            EncodingType.PNG => "image/png",
            EncodingType.JPG => "image/jpeg",
            _ => throw new NotImplementedException("No MIME type exists for this encoding type."),
        };
    }
}
