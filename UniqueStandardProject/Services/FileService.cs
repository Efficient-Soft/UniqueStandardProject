using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Interfaces;

namespace UniqueStandardProject.Services
{
    public class FileService : IFileService
    {
        private readonly IAppLogger<FileService> _logger;
        private readonly IConfiguration _configuration;
        public FileService(IAppLogger<FileService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool WriteImage(Image image, string name, string fileName, string folder)
        {
            try
            {
                #region Path Built

                string pathBuilt = Path.Combine(_configuration.GetSection("Application_Path").Value, $"wwwroot\\images\\{folder}\\");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                #endregion

                string extension = ("." + fileName.Split('.')[^1]).ToLower();
                string path = pathBuilt + name + extension;

                FileDelete(path);

                #region Image Compress
                using Bitmap bmpImage = new(image);
                if (extension == ".jpg" || extension == ".jpeg")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                if (extension == ".png")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                image.Dispose();
                bmpImage.Dispose();

                #endregion

                _logger.LogInformation($"Image Write Successfully [Route:wwwroot\\images\\{folder}\\{name}]");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public bool WriteImage(IFormFile file, string name, string folder)
        {
            try
            {
                #region Path Built

                string pathBuilt = Path.Combine(_configuration.GetSection("Application_Path").Value, $"wwwroot\\images\\{folder}\\");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                #endregion

                string extension = ("." + file.FileName.Split('.')[^1]).ToLower();
                string path = pathBuilt + name + extension;

                FileDelete(path);
                Stream stream = file.OpenReadStream();

                #region Image Compress
                using Bitmap bmpImage = new(stream);
                if (extension == ".jpg" || extension == ".jpeg")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                if (extension == ".png")
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

                    Encoder QualityEncoder = Encoder.Quality;

                    EncoderParameters myEncoderParameters = new(1);

                    EncoderParameter myEncoderParameter = new(QualityEncoder, 40L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpImage.Save(path, jpgEncoder, myEncoderParameters);

                    myEncoderParameters.Dispose();
                }

                stream.Dispose();
                bmpImage.Dispose();

                #endregion

                _logger.LogInformation($"Image Write Successfully [Route:wwwroot\\images\\{folder}\\{name}]");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public bool WriteFile(IFormFile file, string name, string folder)
        {
            try
            {
                #region Path Built

                string pathBuilt = Path.Combine(_configuration.GetSection("Application_Path").Value, $"wwwroot\\files\\{folder}\\");

                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }

                string extension = ("." + file.FileName.Split('.')[^1]).ToLower();
                string path = pathBuilt + name + extension;

                #endregion

                FileDelete(path);
                using FileStream stream = new(path, FileMode.Create);
                file.CopyTo(stream);

                _logger.LogInformation($"Image Write Successfully [Route:wwwroot\\files\\{folder}\\{name}]");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
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
        public bool FileDelete(string path)
        {
            FileInfo file = new(path);
            if (file.Exists) //check file exsit or not 
            {
                file.Delete();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
