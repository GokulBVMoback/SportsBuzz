using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
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
                                            UserRole = role.UserRole
                                        }).ToList();
            return result.ToList();
        }

        public List<UserView> GetUserVersion2()
        {
            var ver2 = _dbContext.UserViews.ToList();
            return ver2;
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

        public string Registration(TblUser user)
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