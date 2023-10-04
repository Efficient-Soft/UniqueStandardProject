using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Areas.Products.Models;

namespace UniqueStandardProject.Interfaces
{
    public interface IFileService
    {
        /// <summary>
        /// Write Image file with IFormFile from html input included compressing feature. Compress Size 40L
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        bool WriteImage(IFormFile file, string name, string folder);

        /// <summary>
        /// Write Image with <b>System.Drawing.Image</b> included compressing feature. Compress Size 40L
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        bool WriteImage(Image image, string name, string fileName, string folder);

        /// <summary>
        /// Write All File Extension not include compressing feature.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="name"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        bool WriteFile(IFormFile file, string name, string folder);

        bool FileDelete(string path);
    }
}
