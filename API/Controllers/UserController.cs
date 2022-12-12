using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DbModels;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserInterface _userService;
        private readonly DbSportsBuzzContext _dbcontext;
        private readonly IPagination _pagination;
        CrudStatus crudStatus;
        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService, IPagination pagination) : base(dbcontext)
        {
            _userService = userService;
            _dbcontext = dbcontext;
            _pagination = pagination;
            crudStatus = new CrudStatus();
        }

        [HttpGet]
        [Authorize]
        public JsonResult UserDetails()
        {
            try
            {
                return new JsonResult(_userService.GetUser().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet]
        [Route("Paginated")]
        public IActionResult GetUsers([FromQuery] PaginationParameters ownerParameters)
        {
            var user =  _pagination.GetUser(ownerParameters);
            var metadata = new
            {
                user.TotalCount,
                user.PageSize,
                user.CurrentPage,
                user.TotalPages,
                user.HasNext,
                user.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(user);
        }

        [HttpPost]
        [Route("registration")]
        public JsonResult Registration(Registration User)
        {
            try
            {
                bool result= _userService.CheckExtistUser(User);
                if(result==false)
                {
                    result=_userService.CheckPassword(User);
                    if (result==true)
                    {
                        string token=_userService.Registration(User);
                        crudStatus.Status = true;
                        crudStatus.Message = token;
                    }
                    else
                    {
                        crudStatus.Status = false;
                        crudStatus.Message = "Password and Confirm password not matched";
                    }
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Your Email already registered. Please Log in";
                }
                    return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public JsonResult LogIn(TblUser logIn)
        {
            try
            {
                string result = _userService.LogIn(logIn);
                if(result!=null)
                {
                    crudStatus.Status=true;
                    crudStatus.Message=result;
                }
                else
                { 
                    crudStatus.Status = false;
                    crudStatus.Message = "Email and Password does not match";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("Forget Password")]
        public JsonResult ForgetPassword(Registration changePassword)
        {
            try
            {
                bool result = _userService.CheckExtistUser(changePassword);
                if (result== true)
                {
                    result = _userService.CheckPassword(changePassword);
                    if (result== true)
                    {
                        _userService.ForgetPassword(changePassword);
                        crudStatus.Status = true;
                        crudStatus.Message = "Password updated successfully";
                    }
                    else
                    {
                        crudStatus.Status = false;
                        crudStatus.Message = "Password and Confirm password not matched";
                    }
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Email doesn't registered. Please Sign up";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("Changing_Active_Status")]
        public JsonResult ChangingActiveStatus(int userId)
        {
            try
            {
                _userService.ChangingActiveStatus(userId);
                crudStatus.Status = true;
                crudStatus.Message = "Active status changed successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("User_Notification")]
        public JsonResult UserNotifications(int userId)
        {
            try
            {
                return new JsonResult(_userService.UserNotifications(userId));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
