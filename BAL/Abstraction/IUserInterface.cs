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
        bool CheckExtistUser(Registration user);
        bool CheckPassword(Registration user);
        bool Registration(Registration user);
        bool LogIn(TblUser login);
        bool ForgetPassword(Registration changePassword);
    }
}
