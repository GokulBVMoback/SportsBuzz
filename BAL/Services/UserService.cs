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
        public UserService(DbSportsBuzzContext dbContext, ILogger<UserService> logger, IEncrypt encrypt)
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
        public Status Registration(Registration user)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(user.Password!);
            string encryptConPassword = encryptService.EncodePasswordToBase64(user.ConfirmPassword!);
            if (encryptPassword == encryptConPassword)
            {
                var obj = new TblUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNum = user.PhoneNum,
                    Password = encryptPassword,
                    UserRole = user.UserRole,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null,
                    Active = true,
                };
                TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == user.Email).FirstOrDefault()!;
                {
                    if (user1.Email != null || _dbContext.TblUsers.Count() == 0)
                    {
                        _dbContext.TblUsers.Add(obj);
                        _dbContext.SaveChanges();
                        return new Status(true, "Registration process done");
                    }
                    return new Status(false, "Your Email already registered. Please Log in");
                }
            }
            else
            {
                return new Status(false, "Password and Confirm password not matched");
            }
        }
        public Status LogIn(LogIn login)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(login.Password!);
            TblUser user = _dbContext.TblUsers.Where(x => x.Email == login.Email && x.Password == encryptPassword).FirstOrDefault()!;
            if (user != null)
            {
                return new Status(false, "Login successfully");
            }
            return new Status(false, "Email and Password doesnt match");
        }
        public Status ForgetPassword(ChangePassword changePassword)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(changePassword.Password!);
            string encryptConPassword = encryptService.EncodePasswordToBase64(changePassword.ConfirmPassword!);
            if (encryptPassword == encryptConPassword)
            {
                TblUser tblUser = _dbContext.TblUsers.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
                tblUser!.Password = encryptPassword;
                tblUser.UpdatedDate = DateTime.Now;
                if (tblUser != null)
                {
                    _dbContext.Entry(tblUser).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return new Status(false, "Password updated successfully");
                }
                return new Status(false, "Email doesn't registered. Please Sign up");
            }
            else
            {
                return new Status(false, "Password and Confirm password not matched");
            }
        }
    }
}