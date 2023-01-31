using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;

namespace BAL.Services
{
    public class TeamMemberService : ITeamMember
    {
        private readonly DbSportsBuzzContext _dbContext;
        public TeamMemberService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<PlayerList> GetTeamMember()
        {
            List<PlayerList> result = (from player in _dbContext.TblTeamMembers
                                       join team in _dbContext.TblTeams on player.TeamId equals team.TeamId
                                       orderby player.TeamId
                                       select new PlayerList
                                       {
                                           MemberId = player.MemberId,
                                           TeamName = team.TeamName,
                                           PlayerFirstName = player.PlayerFirstName,
                                           PlayerLastName = player.PlayerLastName,
                                           Age = player.Age,
                                           JerseyNo = player.JerseyNo,
                                           State = player.State,

                                       }).ToList();
            return result.ToList();
        }

        public List<PlayerList> MyTeamMembers(int? id)
        {
            List<PlayerList> result = (from player in _dbContext.TblTeamMembers
                                       join team in _dbContext.TblTeams on player.TeamId equals team.TeamId
                                       join user in _dbContext.TblUsers on team.UserId equals user.UserId
                                       orderby player.TeamId
                                       where team.TeamId == id
                                       select new PlayerList
                                       {
                                           MemberId = player.MemberId,
                                           TeamName = team.TeamName,
                                           PlayerFirstName = player.PlayerFirstName,
                                           PlayerLastName = player.PlayerLastName,
                                           Age = player.Age,
                                           JerseyNo = player.JerseyNo,
                                           State = player.State,

                                       }).ToList();
            return result.ToList();
        }

        public PlayerList GetTeamMemberbyId(int? id)
        {
            PlayerList result = (from player in _dbContext.TblTeamMembers
                                       join team in _dbContext.TblTeams on player.TeamId equals team.TeamId
                                       join user in _dbContext.TblUsers on team.UserId equals user.UserId
                                       orderby player.TeamId
                                       where player.MemberId == id
                                       select new PlayerList
                                       {
                                           MemberId = player.MemberId,
                                           TeamName = team.TeamName,
                                           PlayerFirstName = player.PlayerFirstName,
                                           PlayerLastName = player.PlayerLastName,
                                           Age = player.Age,
                                           JerseyNo = player.JerseyNo,
                                           State = player.State,

                                       }).FirstOrDefault()!;
            return result;
        }

        public void AddTeamMember(TblTeamMember player)
        {
            _dbContext.TblTeamMembers.Add(player);
            _dbContext.SaveChanges();            
        }

        public bool CheckExtistTeamMember(int memberID)
        {
            TblTeamMember player = _dbContext.TblTeamMembers.Where(x => x.MemberId == memberID).FirstOrDefault()!;
            return player != null;
        }

        public void  EditTeamMember(TblTeamMember player)
        {
            TblTeamMember player2 = _dbContext.TblTeamMembers.Where(x => x.MemberId==player.MemberId).FirstOrDefault()!;
            player2.PlayerFirstName = player.PlayerFirstName;
            player2.PlayerLastName = player.PlayerLastName;
            player2.Age = player.Age;
            player2.JerseyNo = player.JerseyNo;             
            _dbContext.Entry(player2).State = EntityState.Modified;
            _dbContext.SaveChanges();           
        }

        public void DeleteTeamMember(int memberID)
        {
            TblTeamMember teamMember1 = _dbContext.TblTeamMembers.Where(x => x.MemberId == memberID).FirstOrDefault()!;
            _dbContext.TblTeamMembers.Remove(teamMember1);
            _dbContext.SaveChanges();           
        }
    }
}
