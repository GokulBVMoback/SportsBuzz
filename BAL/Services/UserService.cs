using BAL.Abstraction;
using Entities.Models;
using EnvDTE;
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
using Twilio.TwiML.Messaging;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BAL.Services
{
    public class UserService : IUserInterface,IPagination
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
            List<UserDisplay> result =  (from user in _dbContext.TblUsers
                                        join role in _dbContext.TblUserRoles on user.UserRole equals role.UserRoleId
                                        orderby user.UserId
                                        select new UserDisplay
                                        {
                                            UserId = user.UserId,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            Email = user.Email,
                                            PhoneNum = user.PhoneNum,
                                            UserRole = role.UserRole
                                        }).ToList();
            return result.ToList();
        }
        public IQueryable<TblUser> GetAll()
        {
            return this. _dbContext.Set<TblUser>()
                .AsNoTracking();
        }
        public PagedList<TblUser> GetUser(PaginationParameters ownerParameters)
        {
            bool IsDescending=ownerParameters.IsDescending;
            if(IsDescending==false)
            return PagedList<TblUser>.ToPagedList(GetAll().OrderBy(on => on.CreatedDate),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
            else
            return PagedList<TblUser>.ToPagedList(GetAll().OrderByDescending(on => on.CreatedDate),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }

        public bool CheckExtistUser(Registration user)
        {
            TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == user.Email).FirstOrDefault()!;
            return user1!=null;
        }

        public bool CheckPassword(Registration user)
        {
            return user.Password == user.ConfirmPassword;
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
 
        public void ForgetPassword(Registration changePassword)
        {
            TblUser user1 = _dbContext.TblUsers.Where(x => x.Email == changePassword.Email).FirstOrDefault()!;
            user1!.Password = encryptService.EncodePasswordToBase64(changePassword.Password!);
            user1.UpdatedDate = DateTime.Now;
            _dbContext.Entry(user1).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void ChangingActiveStatus(int userId)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == userId).FirstOrDefault()!;
            user.Active=user.Active==true?false:true;
            user.UpdatedDate = DateTime.Now;
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public List<string> UserNotifications(int userId)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == userId).FirstOrDefault()!;
            if (user.UserRole == 1)
            {
                TblTeam manager = _dbContext.TblTeams.Where(x => x.UserId == userId).FirstOrDefault()!;
                List<string> notifications = new List<string>();
                return notifications;
            }
            else
            {
                List<BookedGroundView> notification = _dbContext.BookedGroundViews.Where(x => x.UserId == userId).ToList()!;
                List<string> notifications = new List<string>();
                foreach (var items in notification)
                {
                    string message = "Hi " + items.TeamName + " team booked your ground " + items.Venue + " on " + String.Format($"{items.Date:dd-MM-yyyy}") + " at " + items.Session;
                    notifications.Add(message);
                }
                return notifications;
            }
        }
    }
} 