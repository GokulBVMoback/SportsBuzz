using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface ITeamMember
    {
        List<PlayerList> GetTeamMember();
        bool AddTeamMember(TblTeamMember Player);
        bool CheckExtistTeamMember(TblTeamMember Player);
        bool EditTeamMember(TblTeamMember Player);
        bool DeleteTeamMember(TblTeamMember Player);
    }
}
