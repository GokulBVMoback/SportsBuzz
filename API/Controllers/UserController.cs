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
    public class UserController : BaseController
    {
        private readonly IUserInterface _userService;
        
        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService) : base(dbcontext)
        {
            _userService = userService;
        }

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
                    if(result == true)
                    {
                        result= _userService.Registration(User);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result,Message= "Registration process done" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = result, Message = "Password and Confirm password not matched" });
                    }
                }
                    return new JsonResult(new CrudStatus() { Status = false, Message= "Your Email already registered. Please Log in" });
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
                bool result = _userService.LogIn(logIn);
                if(result==true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "Login successfully" });

                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Email and Password doesnt match" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("Forget Password")]
        public JsonResult ForgetPassword(string Mail, Registration changePassword)
        {
            try
            {
                changePassword.Email = Mail;
                bool result = _userService.CheckExtistUser(changePassword);
                if (result == true)
                {
                    result = _userService.CheckPassword(changePassword);
                    if (result == true)
                    {
                        result = _userService.ForgetPassword(changePassword);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = true, Message = "Password updated successfully" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = result, Message = "Password and Confirm password not matched" });
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Email doesn't registered. Please Sign up" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}