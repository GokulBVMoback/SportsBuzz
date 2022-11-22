using BAL.Abstraction;
using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class AddTeamService 
    {
        private readonly DbSportsBuzzContext _dbContext;
        public AddTeamService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }
        //public List<TeamList> GetTeamMember()
        //{
            
        //}

    }
}
