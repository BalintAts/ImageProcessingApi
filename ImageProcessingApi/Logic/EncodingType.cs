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
        public static string GetExtension(this EncodingType encodingType)
        {
            switch (encodingType)
            {
                case EncodingType.PNG:
                    return ".png";
                case EncodingType.JPG:
                default:
                    return ".jpg";
            }
        }
    }
}
