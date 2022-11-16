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
        CrudStatus Registration(Registration user);
        CrudStatus LogIn(LogIn login);
        CrudStatus ForgetPassword(ChangePassword changePassword);
    }
}
