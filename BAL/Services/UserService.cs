using BAL.Abstraction;
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
        private readonly IConfiguration _config;

        public UserService(DbSportsBuzzContext dbContext, IEncrypt encrypt,IConfiguration config)
        {
            encryptService = encrypt;
            _dbContext = dbContext;
            _config=config;
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

        public bool Registration(Registration user)
        {
            user.Password = encryptService.EncodePasswordToBase64(user.Password!);
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = null;
            user.Active = true;
            _dbContext.TblUsers.Add(user);
            _dbContext.SaveChanges();
            return true;
        }

        public string LogIn(TblUser login)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.Email == login.Email && x.Password == encryptService.EncodePasswordToBase64(login.Password!)).FirstOrDefault()!;
            if (user != null)
            {
                var token = GenerateToken(user);
                return token;
            }
            return null!;
        }
        public string GenerateToken(TblUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            TblUserRole role = _dbContext.TblUserRoles.Where(x=>x.UserRoleId==user.UserRole).FirstOrDefault()!;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email!),
                 new Claim(ClaimTypes.NameIdentifier,user.Password!),
                  new Claim(ClaimTypes.Role, role.UserRole!),

            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

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

        public bool ChangingActiveStatus(int userId)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == userId).FirstOrDefault()!;
            if(user.Active==true)
            {
                user.Active = false;
            }
            else
            {
                user!.Active = true;
            }
            user.UpdatedDate = DateTime.Now;
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public List<string> UserNotifications(int userId)
        {
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == userId).FirstOrDefault()!;
            if (user.UserRole == 1)
            {
                TblTeam manager = _dbContext.TblTeams.Where(x => x.UserId == userId).FirstOrDefault()!;
                //List<TblBookGround> notification = _dbContext.TblBookGrounds.Where(x => x.GroundId == manager.GroundId).ToList();
                List<string> notifications = new List<string>();
                //foreach (var items in notification)
                //{
                //    string message = "Hi " + items.TeamId + " booked your ground " + items.GroundId + " on " + items.Date + " at " + items.SessionId;
                //    notifications.Add(message);
                //}
                return notifications;
            }
            else
            {
                TblGround manager = _dbContext.TblGrounds.Where(x => x.UserId == userId).FirstOrDefault()!;
                List<TblBookGround> notification = _dbContext.TblBookGrounds.Where(x => x.GroundId == manager.GroundId).ToList();
                List<string> notifications = new List<string>();
                foreach (var items in notification)
                {
                    TblTeam teamName = _dbContext.TblTeams.Where(x => x.TeamId == items.TeamId).FirstOrDefault()!;
                    TblGround groundname= _dbContext.TblGrounds.Where(x => x.GroundId == items.GroundId).FirstOrDefault()!;
                    TblSession session= _dbContext.TblSessions.Where(x => x.SessionId == items.SessionId).FirstOrDefault()!;

                    string message = "Hi " + teamName.TeamName + " booked your ground " + groundname.Venue + " on " + items.Date + " at " + session.Session;
                    notifications.Add(message);
                }
                return notifications;
            }
        }
    }
} 