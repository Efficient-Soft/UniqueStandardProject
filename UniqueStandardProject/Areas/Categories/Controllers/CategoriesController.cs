using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Areas.UserManage.Models;
using UniqueStandardProject.Data;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Areas.Categories.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly UniqueStandardDbContext _context;

        public CategoriesController(UniqueStandardDbContext context)
        {
            _context = context;
        }

        #region categories

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var list = await _context.Categories.ToListAsync();
            return Ok(new ResponseModel()
            {
                Meta = new { total_count = list.Count },
                Data = list,
                Code = StatusCodes.Status200OK,
                Success = true,
            });
        }

        [HttpPost("category/create")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategories([FromForm] Category model)
        {

            try
            {
                _context.Categories.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Created("/api/categories/category/create", new ResponseModel()
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Data = model,
                        Meta = new { message = "Successfully created" },
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel()
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Success = false,
                        Data = null,
                        Meta = null,
                        Message = "Form data is not correct!"
                    });
                }
            }
            catch (Exception exp)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Data = null,
                    Meta = null,
                    Message = exp.Message

                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();

            return Ok(new ResponseModel()
            {
                Data = category,
                Code = StatusCodes.Status200OK,
                Success = true,
                Message = "Request Success!"

            });
        }

        [HttpPost("category/edit")]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> EditCategories([FromForm] Category model)
        {
            var categories = await _context.Categories.Where(c => c.CategoryId == model.CategoryId).FirstOrDefaultAsync();

            if(categories != null)
            {
                categories.CategoryName = model.CategoryName;
            }

            _context.Attach(categories).State = EntityState.Modified;

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(new ResponseModel()
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Data = model,
                    Meta = new { message = "Successfully edit" }
                });
            }
            else
            {
                return BadRequest(new ResponseModel()
                {
                    Code = StatusCodes.Status400BadRequest,
                    Success = false,
                    Data = null,
                    Meta = null,
                    Message = "Update failed!"
                });
            }
        }


        #endregion
    }
}
