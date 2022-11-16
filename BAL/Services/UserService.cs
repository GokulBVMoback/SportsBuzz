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

        public List<UserDisplay> GetUser()
        {
            List<UserDisplay> result = (from su in _dbContext.TblUsers
                                        join ut in _dbContext.TblUserRoles on su.UserRole equals ut.UserRoleId
                                        orderby su.UserId
                                        select new UserDisplay
                                        {
                                            UserId = su.UserId,
                                            FirstName = su.FirstName,
                                            LastName = su.LastName,
                                            Email = su.Email,
                                            PhoneNum = su.PhoneNum,
                                            UserRole = ut.UserRole,
                                            CreatedDate = su.CreatedDate,
                                            UpdatedDate = su.UpdatedDate,
                                            Active = su.Active
                                        }).ToList();
            return result.ToList();
        }

        public CrudStatus Registration(Registration user)
        {
            string encryptPassword = encryptService.EncodePasswordToBase64(user.Password!);
            string encryptConPassword = encryptService.EncodePasswordToBase64(user.ConfirmPassword!);
            TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == user.Email).FirstOrDefault()!;
            if (user1 is null)
            {
                user1 = user;
                user1.Password = encryptPassword;
                user1.CreatedDate = DateTime.Now;
                user1.UpdatedDate = null;
                user1.Active = true;
                if (encryptPassword == encryptConPassword)
                {
                    _dbContext.TblUsers.Add(user1);
                    _dbContext.SaveChanges();
                    return new CrudStatus() { Status = true, Message = "Registration process done" };
                }
                return new CrudStatus() { Status = false, Message = "Password and Confirm password not matched" };
            }
            else
            {
                return new CrudStatus() { Status = false, Message = "Your Email already registered. Please Log in" };
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
            TblUser tblUser = _dbContext.TblUsers.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
            if (tblUser != null)
            {
                if (encryptPassword == encryptConPassword)
                {
                    tblUser!.Password = encryptPassword;
                    tblUser.UpdatedDate = DateTime.Now;
                    _dbContext.Entry(tblUser).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return new CrudStatus() { Status = true, Message = "Password updated successfully" };
                }
                return new CrudStatus() { Status = false, Message = "Password and Confirm password not matched" };
            }
            else
            {
                return new CrudStatus() { Status = false, Message = "Email doesn't registered. Please Sign up" };
            }
        }
    }
}