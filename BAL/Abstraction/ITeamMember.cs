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
        void  AddTeamMember(TblTeamMember Player);
        bool CheckExtistTeamMember(TblTeamMember Player);
        void EditTeamMember(TblTeamMember Player);
        void DeleteTeamMember(TblTeamMember Player);
    }
}
