using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Areas.Products.Models;
using UniqueStandardProject.Areas.UserManage.Models;
using UniqueStandardProject.Data;

namespace UniqueStandardProject.Areas.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly UniqueStandardDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(UniqueStandardDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }

        #region Products
        [HttpGet]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductList()
        {
            var list = await _context.Products.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total = list.Count },
                Data = list
            });
        }
        #endregion

        #region ProductDetail
        [HttpGet("productDetail")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductDetail(int? productId)
        {
            IQueryable<ProductModel> query = from productdetail in _context.ProductDetails
                                             join products in _context.Products
                                             on productdetail.ProductId equals products.ProductId
                                             select new ProductModel()
                                             {
                                                 DetailId = productdetail.DetailId,
                                                 ProductId = products.ProductId,
                                                 Product = products.Product1,
                                                 Img = productdetail.Img,
                                                 Title = productdetail.Title,
                                                 Description = productdetail.Description
                                             };

            List<ProductModel> list = (productId.HasValue) ? await query.Where(q => q.ProductId == productId.Value).ToListAsync() : await query.ToListAsync();

            return Ok(new ResponseModel()
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = list,
                Meta = new { total_count = list.Count },
            });
        }

        [HttpPost("productDetail/delete")]
        public async Task<IActionResult> DeleteProduct(int detailId, int productId)
        {
            var productDetail = await _context.ProductDetails.Where(p => p.DetailId == detailId && p.ProductId == productId).FirstOrDefaultAsync();

            if (productDetail != null)
            {
                _context.ProductDetails.Remove(productDetail);
                await _context.SaveChangesAsync();
                return Ok(new ResponseModel()
                {
                    Message = "Successfully Deleted!",
                    Code = StatusCodes.Status200OK,
                    Success = true
                });
            }
            else
            {
                return BadRequest(new ResponseModel()
                {
                    Success = false,
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Unsuccessfully!"
                });
            }
        }

        #endregion

        #region ProductImg
        [HttpGet("productImg")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductImgList()
        {
            IQueryable<ProductModel> query = from productdetail in _context.ProductDetails
                                             join products in _context.Products
                                             on productdetail.ProductId equals products.ProductId
                                             select new ProductModel()
                                             {
                                                 ProductId = products.ProductId,
                                                 DetailId = productdetail.DetailId,
                                                 Product = products.Product1,
                                                 Img = productdetail.Img,
                                                 Title = productdetail.Title,
                                             };

            List<ProductModel> list = await query.ToListAsync();

            return Ok(new ResponseModel()
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = list,
                Meta = new { total_count = list.Count },
            });
        }

        [HttpGet("productImgList")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductImgList()
        {
            IQueryable<ProductModel> query = from productdetail in _context.ProductDetails
                                             join products in _context.Products
                                             on productdetail.ProductId equals products.ProductId
                                             join productImg in _context.ProductImgs
                                             on productdetail.DetailId equals productImg.DetailId
                                             select new ProductModel()
                                             {
                                                 ImageId = productImg.ImageId,
                                                 Product = products.Product1,
                                                 Img = productImg.Img,
                                                 Title = productdetail.Title,
                                             };

            List<ProductModel> list = await query.ToListAsync();

            return Ok(new ResponseModel()
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = list,
                Meta = new { total_count = list.Count },
            });
        }

        [HttpGet("productTitle")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductTitle()
        {
            var list = await _context.ProductDetails.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total = list.Count },
                Data = list
            });
        }

        [HttpGet("ImgTitle")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImgTitleChange(int? detailId)
        {
            IQueryable<ProductModel> query = from productdetail in _context.ProductDetails
                                             join productImg in _context.ProductImgs
                                             on productdetail.DetailId equals productImg.DetailId
                                             join products in _context.Products
                                             on productdetail.ProductId equals products.ProductId
                                             select new ProductModel()
                                             {
                                                 ImageId = productImg.ImageId,
                                                 Product = products.Product1,
                                                 DetailId = productdetail.DetailId,
                                                 Img = productImg.Img,
                                                 Title = productdetail.Title,
                                             };

            List<ProductModel> list = (detailId.HasValue) ? await query.Where(q => q.DetailId == detailId.Value).ToListAsync() : await query.ToListAsync();

            return Ok(new ResponseModel()
            {
                Code = StatusCodes.Status200OK,
                Success = true,
                Data = list,
                Meta = new { total_count = list.Count },
            });
        }
        #endregion

        #region Master
        [HttpGet("masterList")]
        public async Task<IActionResult> GetMasterList()
        {
            var list = await _context.MasterTbls.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total = list.Count },
                Data = list
            });
        }
        #endregion

        #region Distributor

        [HttpGet("distributorList")]
        public async Task<IActionResult> GetdistributorList()
        {
            var list = await _context.Distributors.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total = list.Count },
                Data = list
            });
        }

        [HttpPost("distributor/delete")]
        public async Task<IActionResult> DeleteDistributor(int id)
        {
            var distributor = await _context.Distributors.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (distributor != null)
            {
                _context.Distributors.Remove(distributor);
                await _context.SaveChangesAsync();
                return Ok(new ResponseModel()
                {
                    Message = "Successfully Deleted!",
                    Code = StatusCodes.Status200OK,
                    Success = true
                });
            }
            else
            {
                return BadRequest(new ResponseModel()
                {
                    Success = false,
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Unsuccessfully!"
                });
            }
        }

        #endregion

        #region Service

        [HttpGet("serviceList")]
        public async Task<IActionResult> GetServiceList()
        {
            var list = await _context.ServiceTbls.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total = list.Count },
                Data = list
            });
        }

        [HttpPost("service/delete")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            var service = await _context.ServiceTbls.Where(d => d.ServiceId == serviceId).FirstOrDefaultAsync();

            if (service != null)
            {
                _context.ServiceTbls.Remove(service);
                await _context.SaveChangesAsync();
                return Ok(new ResponseModel()
                {
                    Message = "Successfully Deleted!",
                    Code = StatusCodes.Status200OK,
                    Success = true
                });
            }
            else
            {
                return BadRequest(new ResponseModel()
                {
                    Success = false,
                    Code = StatusCodes.Status400BadRequest,
                    Message = "Unsuccessfully!"
                });
            }
        }

        #endregion

        [HttpPost("deleteImg")]
        public IActionResult DeleteImages([FromForm] ProductImgModel model)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            if (model.SelectedImages != null && model.SelectedImages.Any())
            {
                foreach (int imageId in model.SelectedImages)
                {
                    // Retrieve the image from your data store (e.g., a database)
                    var imageToDelete = _context.ProductImgs.FirstOrDefault(i => i.ImageId == imageId);

                    if (imageToDelete != null)
                    {
                        // Delete the image file from the server
                        string imagePathToDelete = Path.Combine(_hostingEnvironment.WebRootPath, imageToDelete.Img);
                        if (System.IO.File.Exists(imagePathToDelete))
                        {
                            System.IO.File.Delete(imagePathToDelete);
                        }

                        // Remove the image from your data store
                        _context.ProductImgs.Remove(imageToDelete);
                    }
                }

                // Save changes to the database
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Redirect to the image listing page
        }




    }
}
