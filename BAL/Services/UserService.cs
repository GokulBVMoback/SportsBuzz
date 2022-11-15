using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class UserService : IUserInterface
    {
        private readonly DbSportsBuzzContext _dbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IEncrypt encryptService;
        public UserService(DbSportsBuzzContext dbContext, ILogger<UserService> logger,IEncrypt encrypt)
        {
            encryptService = encrypt;
            _dbContext = dbContext;
            _logger = logger;
        }
        public List<TblUser> GetUser()
        {
            var users = _dbContext.TblUsers.ToList();
            return users;
        }
        public string Registration(TblUser user)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(user.Password);
            //string encryptConPassword = encryptService.EncodePasswordToBase64(signUp.ConfirmPassword);

            var obj = new TblUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNum = user.PhoneNum,
                Password = encryptPassword,
                //ConfirmPassword = encryptConPassword,
                UserRole = user.UserRole,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                Active = true,

            };
            //if (obj.Password == obj.ConfirmPassword)
            //{
            if (_dbContext.TblUsers.Count() == 0)
            {
                _dbContext.TblUsers.Add(obj);
                _dbContext.SaveChanges();
                return "Sign up successfully";
            }
            foreach (var item in _dbContext.TblUsers)
            {
                if (item.Email != user.Email)
                {
                    _dbContext.TblUsers.Add(obj);
                    _dbContext.SaveChanges();
                    return "Sign up successfully";
                }
            }
            //}
            //else
            //{
            //    return "Password and Confirm password not matched";
            //}
            return "Your Email already registered. Please Log in";
        }
        public bool logIN(LogIn login)
        {
            var obj = new LogIn
            {
                Email = login.Email,
                Password = login.Password,
            };
            foreach (var item in _dbContext.TblUsers)
            {
                string decryptedPassword = encryptService.DecodeFrom64(item.Password);
                if (item.Email == obj.Email && decryptedPassword == obj.Password)
                {
                    return true;
                }
            }
            return false;
        }
        public string ForgetPassword(ChangePassword changePassword)
        {
            {
                string encryptPassword = encryptService.EncodePasswordToBase64(changePassword.Password);
                string encryptConPassword = encryptService.EncodePasswordToBase64(changePassword.ConfirmPassword);
                var obj = new ChangePassword
                {
                    Email = changePassword.Email,
                    Password = encryptPassword,
                    ConfirmPassword = encryptConPassword,
                };
                if (obj.Password == obj.ConfirmPassword)
                {
                    TblUser tblUser = _dbContext.TblUsers.Where(x => x.Email == obj.Email).FirstOrDefault();
                    tblUser.Password = obj.Password;
                    tblUser.UpdatedDate = DateTime.Now;
                    if (tblUser != null)
                    {
                        //_dbContext.SignUps.Update(obj2);
                        _dbContext.Entry(tblUser).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        return "Password updated successfully";
                    }
                    return "Email doesn't registered. Please Sign up";
                }
                else
                {
                    return "Password and Confirm password not matched";
                }
            }
        }
    }
}