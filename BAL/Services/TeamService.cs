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
    public class TeamService : ITeam
    {
        private readonly DbSportsBuzzContext _dbContext;
        public TeamService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TeamList> GetTeam()
        {
            List<TeamList> result = (from team in _dbContext.TblTeams
                                        join sport in _dbContext.TblSportTypes on team.SportType equals sport.SportTypeId
                                        join user in _dbContext.TblUsers on team.UserId equals user.UserId
                                        orderby team.TeamId
                                        select new TeamList
                                        {
                                            TeamId = team.TeamId,   
                                            TeamName= team.TeamName,
                                            City= team.City,
                                            SportType= sport.SportType,
                                            Email= team.Email,
                                            PhoneNum= team.PhoneNum,
                                            FirstName=user.FirstName,
                                            LastName=user.LastName,
                                            CreatedDate= team.CreatedDate,
                                            UpdatedDate= team.UpdatedDate,
                                            Active= team.Active,
                                         }).ToList();
            return result.ToList();
        }

        public List<TeamList> SearchByCity(string City)
        {
            return GetTeam().Where(x => x.City == City).ToList();
        }

        public TeamList SearchByTeamName(string Team)
        {
            return GetTeam().Where(x=>x.TeamName==Team).FirstOrDefault()!;
        }

        public bool CheckExtistTeam(TblTeam team)
        {
            TblTeam team1 = _dbContext.TblTeams.Where(x => x.TeamName == team.TeamName).FirstOrDefault()!;
            if (team1 == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckExtistUserId(TblTeam team) 
        {
            TblTeam team2 = _dbContext.TblTeams.Where(y => y.UserId == team.UserId).FirstOrDefault()!;
            if (team2 == null)
            {
                return false;
            }
            return true;
        }

        public bool TeamRegistration(TblTeam team)
        {
            team.CreatedDate = DateTime.Now;
            team.UpdatedDate = null;
            team.Active = true;
            _dbContext.TblTeams.Add(team);
            _dbContext.SaveChanges();
            return true;
        }

        public bool EditTeam(TblTeam TeamName)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == TeamName.TeamId).FirstOrDefault()!;
            team.TeamName = TeamName.TeamName;
            team.UpdatedDate = DateTime.Now;
            _dbContext.Entry(team).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteTeam(TblTeam team)
        {
            TblTeam team1 = _dbContext.TblTeams.Where(x => x.TeamId==team.TeamId).FirstOrDefault()!;
            _dbContext.TblTeams.Remove(team1);  
            _dbContext.SaveChanges();
            return true;
        }
    }
}
