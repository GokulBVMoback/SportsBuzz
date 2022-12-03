﻿using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace BAL.Services
{
    public class UserService : IUserInterface
    {
        private readonly DbSportsBuzzContext _dbContext;
        private readonly IEncrypt encryptService;
        private readonly IGenarate _genarate;

        public UserService(DbSportsBuzzContext dbContext, IEncrypt encrypt, IGenarate genarate)
        {
            encryptService = encrypt;
            _dbContext = dbContext;
            _genarate = genarate;
        }

        public List<UserDisplay> GetUser()
        {
            List<UserDisplay> result = (from user in _dbContext.TblUsers
                                        join role in _dbContext.TblUserRoles on user.UserRole equals role.UserRoleId
                                        orderby user.UserId
                                        select new UserDisplay
                                        {
                                            UserId = user.UserId,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            Email = user.Email,
                                            PhoneNum = user.PhoneNum,
                                            UserRole = role.UserRole,
                                            CreatedDate = user.CreatedDate,
                                            UpdatedDate = user.UpdatedDate,
                                            Active = user.Active
                                        }).ToList();
            return result.ToList();
        }

        public bool CheckExtistUser(Registration user)
        {
            TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == user.Email).FirstOrDefault()!;
            if (user1 is null)
            {
                return false;
            }
            return true;
        }

        public bool CheckPassword(Registration user)
        {
            if (user.Password == user.ConfirmPassword)
            {
                return true;
            }
            return false;
        }

        public string Registration(Registration user)
        {
            user.Password = encryptService.EncodePasswordToBase64(user.Password!);
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = null;
            user.Active = true;
            _dbContext.TblUsers.Add(user);
            _dbContext.SaveChanges();
            var token = _genarate.GenerateToken(user);
            return token;
        }

        public string LogIn(TblUser login)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.Email == login.Email && x.Password == encryptService.EncodePasswordToBase64(login.Password!)).FirstOrDefault()!;
            if (user != null)
            {
                login.UserRole=user.UserRole;
                var token = _genarate.GenerateToken(login);
                return token;
            }
            return null!;
        }
 
        public bool ForgetPassword(Registration changePassword)
        {
            TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
            user1!.Password = encryptService.EncodePasswordToBase64(changePassword.Password!);
            user1.UpdatedDate = DateTime.Now;
            _dbContext.Entry(user1).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }
    }
} 