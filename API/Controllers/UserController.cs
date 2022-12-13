using AutoMapper;
using AutoMapper.QueryableExtensions;
using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using FluentNHibernate.Automapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DbModels;
using Newtonsoft.Json;
using NHibernate.Mapping;
using Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserInterface _userService;
        private readonly DbSportsBuzzContext _dbcontext;
        private readonly IPagination _pagination;
        private readonly CrudStatus crudStatus;
        public new const string SessionKey = "UserId";
        private readonly IMapper _mapper;

        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService, IMapper mapper, IPagination pagination) : base(dbcontext)
        {
            _userService = userService;
            _dbcontext = dbcontext;
            _pagination = pagination;
            crudStatus = new CrudStatus();
            _mapper=mapper;
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
        [Authorize]
        [Route("Paginated")]
        public IActionResult GetUsers([FromQuery] PaginationParameters ownerParameters)
        {
            var user = _pagination.GetUser(ownerParameters);
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
        [HttpGet]
        [Authorize]
        [MapToApiVersion("2")]
        [Route("V2")]
        public ActionResult<List<UserDisplayV2>> UserDetails2()
        {
            var users = _userService.GetUserVersion2().Select(x=>_mapper.Map<UserDisplayV2>(x));
            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [MapToApiVersion("3")]
        [Route("V3")]
        public ActionResult<List<UserDisplayV2>> UserDetails3()
        {
            var c = new MapperConfiguration(cfg => cfg.CreateProjection<TblUser, UserDisplay>()
                                                      .ForMember(dto => dto.UserRole, conf =>
                                                  conf.MapFrom(ol => ol.UserRoleNavigation!.UserRole)));
            return Ok(_dbcontext.TblUsers.ProjectTo<UserDisplay>(c).ToList());
        }

        [HttpGet]
        [Authorize]
        [MapToApiVersion("4")]
        [Route("V4")]
        public List<UserDisplayV2> UserDetails4()
        {
            return AutoMapper<UserView, UserDisplayV2>.MapList(_userService.GetUserVersion2());
        }

        [HttpPost]
        [Route("registration")]
        public JsonResult Registration(Registration user)
        {
            try
            {
                var userdto = AutoMapper<Registration, TblUser>.MapClass(user);
                bool result = _userService.CheckExtistUser(user);
                if(result==false)
                {
                    result=_userService.CheckPassword(user);
                    if (result==true)
                    {
                        Tuple<string, int> token = _userService.Registration(userdto);
                        HttpContext.Session.SetInt32(SessionKey, token.Item2);
                        LoginId(SessionKey);
                        crudStatus.Status = true;
                        crudStatus.Message = token.Item1;
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
        public JsonResult LogIn(LogIn logIn)
        {
            try
            {
                var logIndto = AutoMapper<LogIn, TblUser>.MapClass(logIn);
                Tuple<string, int> result = _userService.LogIn(logIndto);
                if (result != null)
                {
                    HttpContext.Session.SetInt32(SessionKey, result.Item2);
                    LoginId(SessionKey);
                    crudStatus.Status = true;
                    crudStatus.Message = result.Item1;
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
        public JsonResult ForgetPassword(ForgotPassword changePassword)
        {
            try
            {
                var changePassworddto = AutoMapper<ForgotPassword, Registration>.MapClass(changePassword);
                bool result = _userService.CheckExtistUser(changePassworddto);
                if (result== true)
                {
                    result = _userService.CheckPassword(changePassworddto);
                    if (result== true)
                    {
                        _userService.ForgetPassword(changePassworddto);
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
        [Authorize]
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
        [Authorize]
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
