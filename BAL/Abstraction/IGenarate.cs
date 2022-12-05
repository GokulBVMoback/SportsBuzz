using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IGenarate
    {
        string GenerateToken(TblUser user);
    }
}
