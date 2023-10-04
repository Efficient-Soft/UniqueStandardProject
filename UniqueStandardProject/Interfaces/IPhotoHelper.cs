using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace UniqueStandardProject.Interfaces
{
    public interface IPhotoHelper
    {
        bool ExtensionCheck(IFormFile file);
        ImageCodecInfo GetEncoder(ImageFormat format);
    }
}
