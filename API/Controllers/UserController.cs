using AutoMapper;
using AutoMapper.QueryableExtensions;
using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using EnvDTE;
using FluentNHibernate.Automapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Models;
using Models.DbModels;
using Newtonsoft.Json;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Mapping;
using Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    ///<summary>
    ///[ApiController]attribute makes model validation errors automatically trigger an HTTP 400 response
    ///</summary>
    
    [ApiController]
    ///Api consist User controller classes that drive from the controllerBase
    
    public class UserController : BaseController
    {
        /// <summary>
        /// By the dependency injection we are calling all the methods 
        /// </summary>
        
        private readonly IUserInterface _userService;
        private readonly DbSportsBuzzContext _dbcontext;

        /// <summary>
        /// data source with lots of records and we need to display those records in a view 
        /// We can display all the records in a view at once.
        /// </summary>
       
        private readonly IPagination _pagination;
        private readonly CrudStatus crudStatus;

        /// <summary>
        /// sessionkey: communications session between a user and another computer or between two computers.
        /// </summary>
        
        public new const string SessionKey = "UserId";
        public new const string SessionToken = "Token";
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// In this constructor we are passing these parameters
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="userService"></param>
        /// <param name="mapper"></param>
        /// <param name="pagination"></param>
        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService, IMapper mapper, IPagination pagination, ILogger<UserController> logger) : base(dbcontext)
        {
            _logger= logger;
            _userService = userService;
            _dbcontext = dbcontext;
            _pagination = pagination;
            crudStatus = new CrudStatus();
            _mapper=mapper;
        }

        /// <summary>
        /// calling GetUser() method  from the UserService
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("UserDetails")]

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
        [Route("MyDetails")]
        public JsonResult MyDetails(int? id)
        {
            try
            {
                return new JsonResult(_userService.MyDetails(id));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        /// <summary>
        /// calling GetUser method  from the UserService
        /// </summary>
        /// <param name="ownerParameters"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
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

        /// <summary>
        /// calling GetUserVersion2() method from the UserService
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [MapToApiVersion("2")]
        [Route("V2")]
        public ActionResult<List<UserDisplayV2>> UserDetails2()
        {
            var users = _userService.GetUserVersion2().Select(x=>_mapper.Map<UserDisplayV2>(x));
            return Ok(users);
        }

        /// <summary>
        /// Joining two tables by automapper without linq
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
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
        [Authorize(Policy = "Admin")]
        [MapToApiVersion("4")]
        [Route("V4")]
        public List<UserDisplayV2> UserDetails4()
        {
            return AutoMapper<UserView, UserDisplayV2>.MapList(_userService.GetUserVersion2());
        }

        /// <summary>
        /// Calling Registration() method from UserController
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Registration")]
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
                        crudStatus.Id = token.Item2;
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

        /// <summary>
        /// calling LogIn() method from the UserController
        /// </summary>
        /// <param name="logIn"></param>
        /// <returns></returns>
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
                    HttpContext.Session.SetString(SessionToken, result.Item1);
                    LoginId(SessionKey);
                    crudStatus.Status = true;
                    crudStatus.Message = result.Item1;
                    crudStatus.Id = result.Item2;
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

        /// <summary>
        /// Calling ForgetPassword() method from UserService
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>

        [HttpPut("ForgetPassword")]
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

        /// <summary>
        /// calling ChangingActiveStatus(userId) method from the UserService
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("Changing_Active_Status")]
        [Authorize]
        public JsonResult ChangingActiveStatus(int? id)
        {
            try
            {
                _userService.ChangingActiveStatus(id);
                TblUser user=_dbcontext.TblUsers.Where(x => x.UserId == id).FirstOrDefault()!;
                crudStatus.Status = true;
                if(user.Active==true)
                {
                    crudStatus.Message = "Activated";
                }
                else
                {
                    crudStatus.Message = "Deactivated";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling UserNotifications() method from the UserService
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserNotifications")]
        [Authorize]
        public JsonResult UserNotifications(int? id)
        {
            try
            {
                return new JsonResult(_userService.UserNotifications(id));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        [Route("LogOut")]
        [Authorize]
        public ActionResult LogOut()
        {
            try
            {
                string token = GetToken(SessionToken)!;
                HttpContext.Session.SetString(SessionToken, string.Empty);
                HttpContext.Session.Clear();
                HttpContext.SignOutAsync(token);
                return RedirectToAction("LogIn");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
