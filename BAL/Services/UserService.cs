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
        private readonly IEncrypt encryptService;

        public UserService(DbSportsBuzzContext dbContext, IEncrypt encrypt)
        {
            encryptService = encrypt;
            _dbContext = dbContext;
        }

        public List<TblUser> GetUser()
        {
            var users = _dbContext.TblUsers.ToList();
            return users;
        }

        public CrudStatus Registration(Registration user)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(user.Password!);
            string encryptConPassword = encryptService.EncodePasswordToBase64(user.ConfirmPassword!);
            if (encryptPassword == encryptConPassword)
            {
                TblUser obj = new TblUser
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
                if (user1 is null)
                {
                    _dbContext.TblUsers.Add(obj);
                    _dbContext.SaveChanges();
                    return new CrudStatus() { Status = true, Message = "Registration process done" };
                }
                return new CrudStatus() { Status = false, Message = "Your Email already registered. Please Log in" };

            }
            else
            {
                return new CrudStatus() { Status = false, Message = "Password and Confirm password not matched" };
            }
        }

        public CrudStatus LogIn(LogIn login)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(login.Password!);
            TblUser user = _dbContext.TblUsers.Where(x => x.Email == login.Email && x.Password == encryptPassword).FirstOrDefault()!;
            if (user != null)
            {
                return new CrudStatus() { Status = true, Message = "Login successfully" };
            }
            return new CrudStatus() { Status = false, Message = "Email and Password doesnt match" };
        }

        public CrudStatus ForgetPassword(ChangePassword changePassword)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(changePassword.Password!);
            string encryptConPassword = encryptService.EncodePasswordToBase64(changePassword.ConfirmPassword!);
            if (encryptPassword == encryptConPassword)
            {
                TblUser tblUser = _dbContext.TblUsers.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
                if (tblUser != null)
                {
                    tblUser!.Password = encryptPassword;
                    tblUser.UpdatedDate = DateTime.Now;
                    _dbContext.Entry(tblUser).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return new CrudStatus() { Status = true, Message = "Password updated successfully" };
                }
                return new CrudStatus() { Status = false, Message = "Email doesn't registered. Please Sign up" };
            }
            else
            {
                return new CrudStatus() { Status = false, Message = "Password and Confirm password not matched" };
            }
        }
    }
}