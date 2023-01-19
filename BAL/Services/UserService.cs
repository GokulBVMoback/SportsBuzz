using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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
                                            UserRole = role.UserRole,
                                            Active = user.Active
                                        }).ToList();
            return result.ToList();
        }

        public UserDisplay MyDetails(int? id)
        {
            UserDisplay result = (from user in _dbContext.TblUsers
                                        join role in _dbContext.TblUserRoles on user.UserRole equals role.UserRoleId
                                        orderby user.UserId
                                        where user.UserId == id
                                        select new UserDisplay
                                        {
                                            UserId = user.UserId,
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            Email = user.Email,
                                            PhoneNum = user.PhoneNum,
                                            UserRole = role.UserRole,
                                            Active= user.Active
                                        }).FirstOrDefault()!;
            return result;
        }

        public IQueryable<UserView> GetAll()
        {
            return this. _dbContext.Set<UserView>()
                .AsNoTracking();
        }
        public PagedList<UserView> GetUser(PaginationParameters ownerParameters)
        {
            bool IsDescending=ownerParameters.IsDescending;
            if(IsDescending==false)
            return PagedList<UserView>.ToPagedList(GetAll().OrderBy(on => on.CreatedDate),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
            else
            return PagedList<UserView>.ToPagedList(GetAll().OrderByDescending(on => on.CreatedDate),
                ownerParameters.PageNumber,
                ownerParameters.PageSize);
        }

        public List<UserView> GetUserVersion2()
        {
            var ver2 = _dbContext.UserViews.ToList();
            return ver2;
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

        public Tuple<string, int> Registration(TblUser user)
        {
            user.Password = encryptService.EncodePasswordToBase64(user.Password!);
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = null;
            user.Active = true;
            _dbContext.TblUsers.Add(user);
            _dbContext.SaveChanges();
            var token = _genarate.GenerateToken(user);
            Tuple<string, int> myid = new Tuple<string, int>(token, user.UserId);
            return myid;
            //return token;
        }

        public Tuple<string, int> LogIn(TblUser login)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.Email == login.Email && x.Password == encryptService.EncodePasswordToBase64(login.Password!)).FirstOrDefault()!;
            if (user != null)
            {
                login.UserRole = user.UserRole;
                var token = _genarate.GenerateToken(login);
                Tuple<string, int> myid = new Tuple<string, int>(token, user.UserId);
                return myid;
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

        public void ChangingActiveStatus(int? userId)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == userId).FirstOrDefault()!;
            user.Active=user.Active==true?false:true;
            user.UpdatedDate = DateTime.Now;
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public List<string> UserNotifications(int? userId)
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