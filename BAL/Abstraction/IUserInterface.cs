using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IUserInterface
    {
        List<UserDisplay> GetUser();
        List<UserView> GetUserVersion2();

        bool CheckExtistUser(Registration user);
        bool CheckPassword(Registration user);
        string Registration(Registration user);
        string LogIn(TblUser login);
        bool ForgetPassword(Registration changePassword);
        bool ChangingActiveStatus(int userId);
        List<string> UserNotifications(int userId);
    }
}
