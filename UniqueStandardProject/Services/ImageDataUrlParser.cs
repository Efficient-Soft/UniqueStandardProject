using System;

namespace UniqueStandardProject.Services
{
    public static class ImageDataUrlParser
    {
        public static bool TryParse(string dataUrl, out byte[] bytes)
        {
            bytes = null;

            if (string.IsNullOrWhiteSpace(dataUrl))
            {
                return false;
            }

            int commaIndex = dataUrl.IndexOf(',');
            if (commaIndex <= 0)
            {
                return false;
            }

            string metadata = dataUrl.Substring(0, commaIndex);
            if (!metadata.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase) ||
                metadata.IndexOf(";base64", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return false;
            }

            try
            {
                bytes = Convert.FromBase64String(dataUrl.Substring(commaIndex + 1));
                return bytes.Length > 0;
            }
            catch (FormatException)
            {
                bytes = null;
                return false;
            }
        }
    }
}
