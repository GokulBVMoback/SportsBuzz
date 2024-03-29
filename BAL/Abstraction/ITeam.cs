﻿using Entities.Models;
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
        bool CheckExtistUserId(TblTeam team);
        void TeamRegistration(TblTeam team);
        void EditTeam(TblTeam TeamName);
        void ChangingActiveStatus(int teamID);
    }
}
