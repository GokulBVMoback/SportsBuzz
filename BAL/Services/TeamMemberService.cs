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

        public bool AddTeamMember(TblTeamMember player)
        {
            _dbContext.TblTeamMembers.Add(player);
            _dbContext.SaveChanges();
            return true;
        }

        public bool CheckExtistTeamMember(TblTeamMember Player)
        {
            TblTeamMember player = _dbContext.TblTeamMembers.Where(x => x.MemberId == Player.MemberId).FirstOrDefault()!;
            if (player is null)
            {
                return false;
            }
            return true;
        }

        public bool EditTeamMember(TblTeamMember Player)
        {
            TblTeamMember player = _dbContext.TblTeamMembers.Where(x => x.MemberId==Player.MemberId).FirstOrDefault()!;
            player.PlayerFirstName = player.PlayerFirstName;
            player.PlayerLastName = player.PlayerLastName;
            player.Age = player.Age;
            player.JerseyNo = player.JerseyNo;             
            _dbContext.Entry(player).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteTeamMember(TblTeamMember player)
        {
            TblTeamMember teamMember1 = _dbContext.TblTeamMembers.Where(x => x.MemberId == player.MemberId).FirstOrDefault()!;
            _dbContext.TblTeamMembers.Remove(teamMember1);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
