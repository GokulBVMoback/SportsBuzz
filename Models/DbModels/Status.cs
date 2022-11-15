using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbModels
{
    public class Status
    {
        bool status;
        string message;
        public Status(bool status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
