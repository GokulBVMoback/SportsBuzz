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
        private readonly DbSportsBuzzContext _dbContext;
        private readonly INotification _notification;
        private readonly ITeam _team;
        public ChallengeTeam(DbSportsBuzzContext dbSportsBuzzContext, ITeam team, INotification notification)
        {
            _dbContext = dbSportsBuzzContext;
            _team = team;
            _notification = notification;
        }

        public List<TeamList> SearchAvailableTeam(SearchAvailableGround availablTeam)
        {
            List<TblChallenge> list = _dbContext.TblChallenges.ToList().Where(x => x.SessionId == availablTeam.SessionId && x.Date == availablTeam.Date && x.Status==true).ToList();
            List<TeamList> Team = _team.SearchByCity(availablTeam.City!).ToList();
            foreach (var items in list)
            {
                Team = Team.Where(x => x.TeamId != items.TeamId1 || x.TeamId != items.TeamId2).ToList();
            }
            return Team;
        }
        public bool ChallengingTeam(TblChallenge challenge)
        {
            _dbContext.TblChallenges.Add(challenge);
            _dbContext.SaveChanges();
            _notification.SendWhatsAppNotification(challenge);
            _notification.SendSMSNotification(challenge);
            return true;
        }

        public bool CheckExtistChallengedDetails(TblChallenge challenge)
        {
            TblChallenge booked = _dbContext.TblChallenges.Where(x => x.SessionId == challenge.SessionId && x.Date == challenge.Date && x.Status==true && x.TeamId1 == challenge.TeamId2 || x.TeamId2 == challenge.TeamId2).FirstOrDefault()!;
            if (booked is null)
            {
                return false;
            }
            return true;
        }
    }
}
