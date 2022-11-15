﻿using Entities.Models;
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
        List<TblUser> GetUser();
        Status Registration(Registration user);
        Status LogIn(LogIn login);
       Status ForgetPassword(ChangePassword changePassword);
    }
}
