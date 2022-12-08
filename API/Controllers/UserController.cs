using AutoMapper;
using AutoMapper.QueryableExtensions;
using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DbModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserInterface _userService;
        private readonly IMapper _mapper;
        private readonly DbSportsBuzzContext _db;


        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService, IMapper mapper) : base(dbcontext)
        {
            _db = dbcontext;
            _userService = userService;
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
        [MapToApiVersion("2")]
        [Route("V2")]
        public ActionResult<List<UserDisplayV2>> UserDetails2()
        {
            var users = _userService.GetUserVersion2().Select(x=>_mapper.Map<UserDisplayV2>(x));
            return Ok(users);
        }

        [HttpGet]
        //[Authorize]
        [MapToApiVersion("3")]
        [Route("V3")]
        public ActionResult<List<UserDisplayV2>> UserDetails3()
        {
            var c = new MapperConfiguration(cfg => cfg.CreateProjection<TblUser, UserDisplay>()
                                                      .ForMember(dto => dto.UserRole, conf =>
                                                  conf.MapFrom(ol => ol.UserRoleNavigation.UserRole)));
            return Ok(_db.TblUsers.ProjectTo<UserDisplay>(c).ToList());
        }

        [HttpPost]
        [Route("registration")]
        public JsonResult Registration(Registration User)
        {
            CrudStatus crudStatus = new CrudStatus();
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
            CrudStatus crudStatus = new CrudStatus();
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
            CrudStatus crudStatus = new CrudStatus();
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
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _userService.ChangingActiveStatus(userId);
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
            CrudStatus crudStatus = new CrudStatus();
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
