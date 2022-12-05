using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ChallengeTeam
    {
        private readonly DbSportsBuzzContext _dbSportsBuzzContext;
        private readonly ITeam _team;
        public ChallengeTeam(DbSportsBuzzContext dbSportsBuzzContext, ITeam team)
        {
            _dbSportsBuzzContext = dbSportsBuzzContext;
            _team = team;
        }

        public List<TeamList> ChechkAvilableTeam(SearchAvailableGround availablTeam)
        {
            List<TblChallenge> list = _dbSportsBuzzContext.TblChallenges.ToList().Where(x => x.SessionId == availablTeam.SessionId && x.Date == availablTeam.Date).ToList();
            List<TeamList> Team = _team.SearchByCity(availablTeam.City!).ToList();
            foreach (var items in list)
            {
                Team = Team.Where(x => x.TeamId != items.TeamId1 && x.TeamId != items.TeamId2).ToList();
            }
            return Team;
        }
        public bool ChallengingTeam()
        {
            return true;
        }
    }
}
