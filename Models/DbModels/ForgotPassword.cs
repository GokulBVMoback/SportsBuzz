using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class ForgotPassword:LogIn
    {
        public string? ConfirmPassword { get; set; } 
    }
}
