using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueStandardProject.Areas.UserManage.Models;
using UniqueStandardProject.Data;
using UniqueStandardProject.Entities;

namespace UniqueStandardProject.Areas.UserManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UniqueStandardDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UniqueStandardDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region user

        [HttpPost("create-user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateUser([FromForm] UserInfoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = new()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                        PhoneNumberConfirmed = true,
                        EmailConfirmed = true
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        UserInfo userInfo = new()
                        {
                            UserId = user.Id,
                            FullName = model.FullName,
                            DetailAddress = model.DetailAddress,
                            Phone = model.Phone,
                            Status = model.Status
                        };

                        _context.UserInfos.Add(userInfo);
                        await _context.SaveChangesAsync();

                        return Ok(new ResponseModel()
                        {
                            Success = true,
                            Code = StatusCodes.Status201Created,
                            Data = new { user = userInfo },
                            Message = "Response successfully!"
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                        {
                            Code = StatusCodes.Status500InternalServerError,
                            Error = "User role added fail!"
                        });
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Error = "User registration fail!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Error = ex.Message
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            List<UserInfoModel> userList = new();

            var users = from aspuser in _context.AspNetUsers
                        join userInfo in _context.UserInfos
                        on aspuser.Id equals userInfo.UserId
                        select new UserInfoModel()
                        {
                            UserId = userInfo.UserId,
                            UserName = aspuser.UserName,
                            Email = aspuser.Email,
                            Image = userInfo.Image,
                            FullName = userInfo.FullName,
                            DetailAddress = userInfo.DetailAddress,
                            Phone = userInfo.Phone,
                            Status = userInfo.Status,
                        };

            userList = await users.ToListAsync();

            return Ok(new ResponseModel()
            {
                Success = true,
                Code = StatusCodes.Status200OK,
                Meta = new { total_count = userList.Count },
                Data = userList,
                Message = "Response successfully!"
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(string id)
        {
            UserInfoModel user = new();

            var users = from aspuser in _context.AspNetUsers
                        join userInfo in _context.UserInfos
                        on aspuser.Id equals userInfo.UserId
                        select new UserInfoModel()
                        {
                            UserId = userInfo.UserId,
                            UserName = aspuser.UserName,
                            Email = aspuser.Email,
                            Image = userInfo.Image,
                            FullName = userInfo.FullName,
                            DetailAddress = userInfo.DetailAddress,
                            Phone = userInfo.Phone,
                            Status = userInfo.Status,
                        };

            user = await users.FirstOrDefaultAsync();

            return Ok(new ResponseModel()
            {
                Success = true,
                Code = StatusCodes.Status200OK,
                Data = user,
                Message = "Response successfully!"
            });

        }

        [HttpPost("edit-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> EditUser([FromForm] UserInfoModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AspNetUser aspNetUser = await _context.AspNetUsers.FindAsync(model.UserId);
                    aspNetUser.UserName = model.UserName;
                    aspNetUser.Email = model.Email;
                    aspNetUser.NormalizedEmail = model.Email.ToUpper();
                    aspNetUser.PhoneNumber = model.Phone;
                    _context.Attach(aspNetUser).State = EntityState.Modified;

                    UserInfo userInfo = await _context.UserInfos.FindAsync(model.UserId);
                    userInfo.UserId = model.UserId;
                    userInfo.FullName = model.FullName;
                    userInfo.DetailAddress = model.DetailAddress;
                    userInfo.Phone = model.Phone;
                    userInfo.Status = model.Status;
                    _context.Attach(userInfo).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return Ok(new ResponseModel()
                    {
                        Success = true,
                        Code = StatusCodes.Status200OK,
                        Data = new { Aspuser = aspNetUser, user = userInfo },
                        Message = "Response successfully!"
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Error = "User update fail!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Error = ex.Message

                });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new ResponseModel()
            {
                Success = true,
                Code = StatusCodes.Status200OK,
                Data = "User logged out successfully!"
            });

        }
        #endregion
    }
}
