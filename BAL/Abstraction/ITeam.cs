using Entities.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface ITeam
    {
        List<TeamList> GetTeam();
        List<TeamList> SearchByCity(string City);
        TeamList SearchByTeamName(string Team);
        bool CheckExtistTeam(TblTeam team);
        bool CheckExtistTeam(string team);
        bool CheckExtistUserId(TblTeam team);
        bool TeamRegistration(TblTeam team);
        bool EditTeam(TblTeam TeamName, string NewTeamName);
        bool DeleteTeam(TblTeam TeamName);   

    }   
}
