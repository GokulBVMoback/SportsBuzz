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
        Tuple<string, int> LogIn(TblUser login);
        void ForgetPassword(Registration changePassword);
        void ChangingActiveStatus(int userId);
        List<string> UserNotifications(int userId);
    }
}
