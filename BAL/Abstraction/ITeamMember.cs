﻿using Entities.Models;
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
        List<PlayerList> MyTeamMembers(int? id);
        PlayerList GetTeamMemberbyId(int? id);
        void AddTeamMember(TblTeamMember Player);
        bool CheckExtistTeamMember(int memberID);
        void EditTeamMember(TblTeamMember Player);
        void DeleteTeamMember(int memberID);
    }
}
