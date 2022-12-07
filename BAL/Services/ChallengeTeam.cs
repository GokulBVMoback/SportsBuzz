using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ChallengeTeam : IChallengeTeam
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
            List<TeamList> Team1 = new List<TeamList>();
            foreach (var items in list)
            {
                TeamList Team123 = Team.Where(x => x.TeamId != items.TeamId1 || x.TeamId != items.TeamId2).FirstOrDefault()!;
                Team1.Add(Team123);
            }
            return Team1;
        }
        public bool ChallengingTeam(TblChallenge challenge)
        {
            challenge.Status = null;
            _dbContext.TblChallenges.Add(challenge);
            _dbContext.SaveChanges();
            Notification notification = GenerateMessage(challenge);
            _notification.SendWhatsAppNotification(notification);
            _notification.SendSMSNotification(notification);
            return true;
        }

        public Notification GenerateMessage(TblChallenge challenge)
        {
            Notification notification = new Notification();
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == challenge.TeamId1).FirstOrDefault()!;
            TblTeam team2 = _dbContext.TblTeams.Where(x => x.TeamId == challenge.TeamId2).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == challenge.SessionId).FirstOrDefault()!;
            notification.Message = "Hello " + team.TeamName + " team challenged you on " + challenge.Date + " at " + session.Session;
            notification.PhoneNum = team2.PhoneNum.ToString();
            return notification;
        }

        public void AcceptTeamRequest(int challengeId)
        {
            TblChallenge react= _dbContext.TblChallenges.Where(x => x.ChallengeId == challengeId).FirstOrDefault()!;
            react.Status = true;
            _dbContext.Entry(react).State = EntityState.Modified;
            _dbContext.SaveChanges();

        }
        public void RejectTeamRequest(int challengeId)
        {
            TblChallenge react = _dbContext.TblChallenges.Where(x => x.ChallengeId == challengeId).FirstOrDefault()!;
            react.Status = false;
            _dbContext.Entry(react).State = EntityState.Modified;
            _dbContext.SaveChanges();

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
