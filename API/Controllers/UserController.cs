using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DbModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserInterface _userService;
        private readonly DbSportsBuzzContext dbContext;
        public UserController(DbSportsBuzzContext dbcontext, ILogger<UserService> logger, IUserInterface userService)
        {
            dbContext = dbcontext;
            _logger = logger;
            _userService = userService;
        }
         // GET: api/<UserController>
        [HttpGet]
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

        // POST api/<UserController>
        [HttpPost]
        [Route("registration")]
        public JsonResult Registration(Registration User)
        {
            try
            {
                return new JsonResult(_userService.Registration(User));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route("LogIn")]
        public JsonResult LogIn( LogIn logIn)
        {
            try
            {
               return new JsonResult(_userService.LogIn(logIn));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{Mail}")]
        public JsonResult ForgetPassword(string Mail, ChangePassword changePassword)
        {
            try
            {
                changePassword.Email = Mail;
                return new JsonResult(_userService.ForgetPassword(changePassword));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
