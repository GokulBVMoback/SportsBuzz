﻿using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
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
        public const string SessionKey = "UserId";

        public UserController(DbSportsBuzzContext dbcontext, IUserInterface userService) : base(dbcontext)
        {
            _userService = userService;
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
                Tuple<string, int> result = _userService.LogIn(logIn);
                if (result!=null)
                {                    
                    HttpContext.Session.SetInt32(SessionKey,result.Item2);
                    LoginId(SessionKey);
                    crudStatus.Status=true;
                    crudStatus.Message=result.Item1;
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
