using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Services
{
    public class PhotoHelper : IPhotoHelper
    {
        /// <summary>
        /// Check Image file extension
        /// It will be return true when JPG, JPEG, PNG
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool ExtensionCheck(IFormFile file)
        {
            string extension = ("." + file.FileName.Split('.')[^1]).ToLower();
            return (extension == ".jpg" || extension == ".jpeg" || extension == ".png"); // Change the extension based on your need
        }

        /// <summary>
        /// Encoder for Imageformat JPG or PNG
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }
}
